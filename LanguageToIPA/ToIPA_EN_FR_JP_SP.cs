using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using MufflerCore.PhonemeLists;
using System.Reflection;

namespace MufflerCore.LanguageToIpa;

// Class to convert English, French, Japanese, and Spanish text to International Phonetic Alphabet (IPA) notation
public class IpaParserEN_FR_JP_SP
{
	private string data_file; // Path to the JSON file containing the conversion rules
	private Dictionary<string, string> 	obj; // Dictionary to store the conversion rules in JSON
	public string uniqueSymbolsString = "";
	public string LanguageCode = "IPA_US"; // set language code
	
	public IpaParserEN_FR_JP_SP(string languageCode) {
		LanguageCode = languageCode;
		// Set the path to the JSON file based on the language dialect
		switch (LanguageCode) {
			case "IPA_US":		data_file = "en_US.json"; break;
			case "IPA_UK":		data_file = "en_UK.json"; break;
			case "IPA_FRENCH":	data_file = "fr_FR.json"; break;
			case "IPA_QUEBEC":	data_file = "fr_QC.json"; break;
			case "IPA_JAPAN":	data_file = "ja.json";	break;
			case "IPA_SPAIN":	data_file = "es_ES.json"; break;
			case "IPA_MEXICO":	data_file = "es_MX.json";	break;
			default:			data_file = "en_US.json"; break;
		}
		// Try to read the JSON file and deserialize it into the obj dictionary
		try {
            // Use the language code to set the data_file path
            this.data_file = GetResourcePath($"{LanguageCode}");
            LoadDictionary();
		}
		catch (FileNotFoundException) {
            // If the file does not exist, log an error and initialize obj as an empty dictionary
            obj = new Dictionary<string, string>();
            throw new Exception($"[IPA Parser] File does not exist: {data_file}");
		}
		catch (Exception ex) {
            // If any other error occurs, log the error and initialize obj as an empty dictionary
            obj = new Dictionary<string, string>();
            throw new Exception($"[IPA Parser] An error occurred while reading the file: {ex.Message}");
		}

		// extraction time
		try {
			SetUniqueSymbolsString();
		}
		catch (Exception ex) {
			throw new Exception($"[IPA Parser] An error occurred while extracting unique phonetics: {ex.Message}");
		}
	}

    // Method to determine the path to the resource
    private string GetResourcePath(string relativePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        var fullPath = Path.Combine(assemblyDirectory, "Dictionaries", relativePath);
        return fullPath;
    }

    // Method to load the dictionary from the JSON file
    private void LoadDictionary()
    {
        if (File.Exists(this.data_file))
        {
            string jsonContent = File.ReadAllText(this.data_file);
            this.obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
        }
        else
        {
            throw new FileNotFoundException($"Dictionary file not found: {this.data_file}");
        }
    }

    /// <summary> Preprocess input string by converting it to lower case and removing certain characters.
    /// <list type="Bullet"><item><c>x</c><param name="x"> - String to preprocess</param></item></list>
    /// </summary> <returns> The preprocessed input string</returns>
    private string Preprocess(string x) {
		x = Regex.Replace(x, @"\n", "");
		return x;
	}

	/// <summary> Function for converting an input string to IPA notation.
	/// <para> THIS IS FOR UI DISPLAY PURPOSES, Hince the DASHED SPACE BETWEEN PHONEMES </para>
	/// <list type="Bullet"><item><c>input</c><param name="input"> - String to convert</param></item></list>
	/// </summary><returns> The input string converted to IPA notation</returns>
    public string ToIPAStringDisplay(string input) {
		// split the string by the spaces between words
        string[] c_w = (Preprocess(input) + " ").Split(" ");
        // the new string to output
		string str = "";
		// iterate over each word in the input string
        foreach (var word in c_w) {
			// if the word is not empty
            if (!string.IsNullOrEmpty(word)) {
				// remove punctuation from the word
				string wordWithoutPunctuation = Regex.Replace(word, @"\p{P}", "");
				wordWithoutPunctuation = wordWithoutPunctuation.ToLower();
				// if the word exists in the dictionary
                if (obj.ContainsKey(wordWithoutPunctuation)) {
					// append the word and its phonetic to the string
                    str += $"( {word} : {obj[wordWithoutPunctuation]} ) ";
                }
				// if not, append the word by itself
                else {
                    str += $"{word} ";
                }
            }
        }
        return str;
    }

	/// <summary>
	/// The same as ToIPAStringDisp but shows the next step where its split by dashes
	/// </summary>
	public string ToIPAStringSpacedDisplay(string input) {
		string str = input;
		List<Tuple<string, List<string>>> parsedStr = ToIPAList(str);
		str = ConvertDictionaryToSpacedPhonetics(parsedStr);
		return str;
	}

	/// <summary> Converts an input string to a dictionary where each word maps to a list of its phonetic symbols.
	/// <param name="input">The input string to convert.</param>
	/// <returns>A dictionary where each word from the input string maps to a list of its phonetic symbols.</returns></summary>
	public List<Tuple<string, List<string>>> ToIPAList(string input) {
		// Split the input string into words
		string[] c_w = (Preprocess(input) + " ").Split(" ");
		// Initialize the result dictionary
		List<Tuple<string, List<string>>> result = new List<Tuple<string, List<string>>>();
		// Iterate over each word in the input string
		foreach (var word in c_w) {
			// If the word is not empty
			if (!string.IsNullOrEmpty(word)) {
				// remove punctuation from the word
				string wordWithoutPunctuation = Regex.Replace(word, @"\p{P}", "");
				wordWithoutPunctuation = wordWithoutPunctuation.ToLower();
				// If the word exists in the obj dictionary
				if (obj.ContainsKey(wordWithoutPunctuation)) {
					// Retrieve the phonetic representation of the word
					string phonetics = obj[wordWithoutPunctuation];
					// Process the phonetic representation to remove unwanted characters
					phonetics = phonetics.Replace("/", "");
					if (phonetics.Contains(",")) {
						phonetics = phonetics.Split(',')[0].Trim();
					}
					phonetics = phonetics.Replace("ˈ", "").Replace("ˌ", "");
					// Initialize a list to hold the phonetic symbols
					List<string> phoneticSymbols = new List<string>();
					// Iterate over the phonetic symbols
					for (int i = 0; i < phonetics.Length; i++) {
						// Check for known combinations of symbols
						if (i < phonetics.Length - 1) {
							// first 
							string possibleCombination = phonetics.Substring(i, 2);
							int index = GetMasterListBasedOnDialect().FindIndex(t => t == possibleCombination);
							if (index != -1) {
								// If a combination is found, add it to the list and skip the next character
								phoneticSymbols.Add(GetMasterListBasedOnDialect()[index]);
								i++;
							} else {
								// If no combination is found, add the current character to the list
								phoneticSymbols.Add(phonetics[i].ToString());
							}
						} else {
							// Add the last character to the list
							phoneticSymbols.Add(phonetics[i].ToString());
						}
					}
					// Add the list of phonetic symbols to the result dictionary
					result.Add(Tuple.Create(word, phoneticSymbols));
				} else {
					// If the word does not exist in the obj dictionary, add an empty list to the result dictionary
					result.Add(Tuple.Create(word, new List<string>()));
				}
			}
		}
		return result;
	}

	/// <summary>
	/// Converts a dictionary of words and their phonetic symbols to a string of spaced phonetics
	/// </summary>
	public string ConvertDictionaryToSpacedPhonetics(List<Tuple<string, List<string>>> inputTupleList) {
		// Initialize a string to hold the result
		string result = "";

		// Iterate over each entry in the dictionary
		foreach (Tuple<string, List<string>> entry in inputTupleList) {
        // If the list has content, join the phonetic symbols with a dash
        // Otherwise, just use the normal word
        string phonetics = entry.Item2.Any() ? string.Join("-", entry.Item2) : entry.Item1;

        // Add the phonetics to the result string
        result += $"{phonetics} ";
		}

		// Return the result string
		return result.Trim();
	}

	/// <summary>
	/// Returns the master list of phonemes for the selected language
	/// </summary>
	public List<string> GetMasterListBasedOnDialect() {
		switch (LanguageCode) {
			case "IPA_UK":      return PhonemeMasterLists.MasterListEN_UK;
			case "IPA_US":      return PhonemeMasterLists.MasterListEN_US;
			case "IPA_SPAIN":   return PhonemeMasterLists.MasterListSP_SPAIN;
			case "IPA_MEXICO":  return PhonemeMasterLists.MasterListSP_MEXICO;
			case "IPA_FRENCH":  return PhonemeMasterLists.MasterListFR_FRENCH;
			case "IPA_QUEBEC":  return PhonemeMasterLists.MasterListFR_QUEBEC;
			case "IPA_JAPAN":   return PhonemeMasterLists.MasterListJP;
			default:            throw new Exception("Invalid language Dialect");
		}
	}

	/// <summary>
	/// Sets the uniqueSymbolsString to the master list of phonemes for the selected language
	/// </summary>
	public void SetUniqueSymbolsString() {
        switch (LanguageCode) {
            case "IPA_UK":      uniqueSymbolsString = string.Join(",", PhonemeMasterLists.MasterListEN_UK); break;
            case "IPA_US":      uniqueSymbolsString = string.Join(",", PhonemeMasterLists.MasterListEN_US); break;
            case "IPA_SPAIN":   uniqueSymbolsString = string.Join(",", PhonemeMasterLists.MasterListSP_SPAIN); break;
            case "IPA_MEXICO":  uniqueSymbolsString = string.Join(",", PhonemeMasterLists.MasterListSP_MEXICO); break;
            case "IPA_FRENCH":  uniqueSymbolsString = string.Join(",", PhonemeMasterLists.MasterListFR_FRENCH); break;
            case "IPA_QUEBEC":  uniqueSymbolsString = string.Join(",", PhonemeMasterLists.MasterListFR_QUEBEC); break;
            case "IPA_JAPAN":   uniqueSymbolsString = string.Join(",", PhonemeMasterLists.MasterListJP); break;
            default:            throw new Exception("Invalid language Dialect");
        }
	}
}