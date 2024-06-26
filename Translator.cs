using MufflerCore.LanguageToIpa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MufflerCore;
public class Translator
{
    private readonly MufflerService MufflerObjectService;
    private readonly IpaParserEN_FR_JP_SP IpaParser; 
    public List<MuffleObject> ActiveMufflers;

    public Translator(IpaParserEN_FR_JP_SP IPAParser, MufflerService MuffleObjectService, List<IMuffleObject> activeMuffleObjects)
    {
        IpaParser = IPAParser;
        MufflerObjectService = MuffleObjectService;

        // configure the active muffle objects in relation to the muffleobject service which defines the valid parameters
        ActiveMufflers = activeMuffleObjects
            .Join(MufflerObjectService.ActiveMuffleObjects,
                  muffleType => muffleType.MuffleSourceItem,
                  muffleObject => muffleObject.MuffleSourceItem,
                  (muffleType, muffleObject) => muffleObject)
            .ToList();
    }

    /// <summary>
    /// Processes a message and returns the garbler translation.
    /// </summary>
    /// <param name="inputMessage">The message to be translated.</param>
    /// <returns>The garbled translation of the message.</returns>
    public string ProcessMessage(string inputMessage)
    {
        string outputStr = "";
        try
        {
            outputStr = ConvertToMuffledSpeech(inputMessage);
        }
        catch (Exception e) 
        {
            throw new Exception($"Error processing message: {e.Message}");
        }

        return outputStr;
    }

    /// <summary>
    /// Converts an IPA spaced message to MuffledSpeech.
    /// </summary>
    /// <param name="inputMessage">The message to be translated.</param>
    /// <returns>The MuffledSpeech translation of the message.</returns>
    public string ConvertToMuffledSpeech(string inputMessage)
    {
        if (ActiveMufflers.All(muffler => muffler.MuffleSourceItem == "None"))
        {
            return inputMessage;
        }

        StringBuilder finalMessage = new StringBuilder();
        bool skipTranslation = false;

        try
        {
            List<Tuple<string, List<string>>> wordsAndPhonetics = IpaParser.ToIPAList(inputMessage);
            foreach (Tuple<string, List<string>> entry in wordsAndPhonetics)
            {
                string word = entry.Item1;
                if (word == "*")
                {
                    skipTranslation = !skipTranslation;
                    finalMessage.Append(word + " ");
                    continue;
                }

                if (!skipTranslation && word.Any(char.IsLetter))
                {
                    bool isAllCaps = word.All(c => !char.IsLetter(c) || char.IsUpper(c));
                    bool isFirstLetterCaps = char.IsUpper(word[0]);
                    string leadingPunctuation = new string(word.TakeWhile(char.IsPunctuation).ToArray());
                    string trailingPunctuation = new string(word.Reverse().TakeWhile(char.IsPunctuation).Reverse().ToArray());
                    string wordWithoutPunctuation = word.Substring(leadingPunctuation.Length, word.Length - leadingPunctuation.Length - trailingPunctuation.Length);
                    string muffledSpeak = entry.Item2.Any() ? ConvertPhoneticsToMuffledSpeech(entry.Item2, isAllCaps, isFirstLetterCaps) : wordWithoutPunctuation;
                    finalMessage.Append(leadingPunctuation + muffledSpeak + trailingPunctuation + " ");
                }
                else
                {
                    finalMessage.Append(word + " ");
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception($"[MufflerGarbleManager] Error converting from IPA Spaced to final output: {e.Message}");
        }
        return finalMessage.ToString().Trim();
    }

    /// <summary>
    /// Converts a list of phonetics to their garbled string text.
    /// </summary>
    /// <param name="phonetics">The list of phonetic symbols to be translated.</param>
    /// <param name="isAllCaps">Indicates if the original word is in all caps.</param>
    /// <param name="isFirstLetterCapitalized">Indicates if the first letter of the original word is capitalized.</param>
    /// <returns>The MuffledSpeech translation of the phonetics.</returns>
    public string ConvertPhoneticsToMuffledSpeech(List<string> phonetics, bool isAllCaps, bool isFirstLetterCapitalized)
    {
        string outputString = "";
        foreach (string phonetic in phonetics)
        {
            try
            {
                int MufflerIndex = ActiveMufflers
                    .Select((mufflerObject, index) => new { mufflerObject, index })
                    .Where(item => item.mufflerObject.MuffleStrengthOfPhoneme.ContainsKey(phonetic) 
                        && !string.IsNullOrEmpty(item.mufflerObject.IpaSymbolSound[phonetic]))
                    .OrderByDescending(item => item.mufflerObject.MuffleStrengthOfPhoneme[phonetic])
                    .FirstOrDefault()?.index ?? -1;
                if (MufflerIndex != -1)
                {
                    string translationSound = ActiveMufflers[MufflerIndex].IpaSymbolSound[phonetic];
                    outputString += translationSound;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"[MufflerGarbleManager] Error converting phonetic {phonetic} to MuffledSpeech: {e.Message}");
            }
        }
        if (isAllCaps)
        {
            outputString = outputString.ToUpper();
        }
        if (isFirstLetterCapitalized && outputString.Length > 0)
        {
            outputString = char.ToUpper(outputString[0]) + outputString.Substring(1);
        }
        return outputString;
    }
}
