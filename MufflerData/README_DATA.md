# You must map the location of your muffled data into a json file for the program to read, otherwise it will not work. Language To IPA


Example below for the language for IPA for the muffle_data.json file that should be referenced.

To transform the json into the data the muffler expects, use:

```csharp
    // read the file
    string json = File.ReadAllText(jsonFilePath);
    // deserialize the json into the obj dictionary
    _gagData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json) ?? new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
```

Below is an example of how to format the json. This format is futureproofed to support all languages included in the core files.
```
{
    "None": {
    },

    "Space Helmet": {
        "p":    { "MUFFLE": 1, "SOUND": "p"  },
        "b":    { "MUFFLE": 1, "SOUND": "b"  },
        "m":    { "MUFFLE": 1, "SOUND": "m"  },
        "w":    { "MUFFLE": 0, "SOUND": "w"  },
        "f":    { "MUFFLE": 0, "SOUND": "f"  },
        "v":    { "MUFFLE": 1, "SOUND": "v"  },
        "θ":    { "MUFFLE": 0, "SOUND": "th"  },
        "ð":    { "MUFFLE": 0, "SOUND": "th"  },
        "t":    { "MUFFLE": 0, "SOUND": "t"  },
        "d":    { "MUFFLE": 1, "SOUND": "h"  },
        "s":    { "MUFFLE": 0, "SOUND": "s"  },
        "z":    { "MUFFLE": 1, "SOUND": "ss"  },
        "n":    { "MUFFLE": 0, "SOUND": "n"  },
        "ɫ":    { "MUFFLE": 1, "SOUND": "l"  },
        "ɹ":    { "MUFFLE": 0, "SOUND": "r"  },
        "ʃ":    { "MUFFLE": 1, "SOUND": "ss"  },
        "ʒ":    { "MUFFLE": 1, "SOUND": "jh"  },
        "dʒ":   { "MUFFLE": 2, "SOUND": "jh"  },
        "tʃ":   { "MUFFLE": 2, "SOUND": "ss"  },
        "j":    { "MUFFLE": 2, "SOUND": "yh"  },
        "k":    { "MUFFLE": 1, "SOUND": "kh"  },
        "ɡ":    { "MUFFLE": 1, "SOUND": "g"  },
        "ŋ":    { "MUFFLE": 1, "SOUND": "nn"  },
        "h":    { "MUFFLE": 0, "SOUND": "h"  },
        
        "eɪ":   { "MUFFLE": 0, "SOUND": "ay"  },
        "ə":    { "MUFFLE": 0, "SOUND": "a"  },
        "ɔ":    { "MUFFLE": 0, "SOUND": "aw"  },
        "æ":    { "MUFFLE": 1, "SOUND": "eh"  },
        "i":    { "MUFFLE": 1, "SOUND": "ee"  },
        "ɛ":    { "MUFFLE": 2, "SOUND": "ah"  },
        "ɝ":    { "MUFFLE": 0, "SOUND": "er"  },
        "ɪə":   { "MUFFLE": 1, "SOUND": "err" },
        "aɪ":   { "MUFFLE": 0, "SOUND": "y"   },
        "ɪ":    { "MUFFLE": 1, "SOUND": "eh"  },
        "oʊ":   { "MUFFLE": 2, "SOUND": "om"  },
        "u":    { "MUFFLE": 0, "SOUND": "oo"  },
        "ɑ":    { "MUFFLE": 0, "SOUND": "o"   },
        "ʊ":    { "MUFFLE": 0, "SOUND": "oo"  },
        "aʊ":   { "MUFFLE": 0, "SOUND": "ow"  },
        "ɔɪ":   { "MUFFLE": 0, "SOUND": "oy"  },
        
        "ɾ":    { "MUFFLE": 2, "SOUND": "eh"  },
        "β":    { "MUFFLE": 1, "SOUND": "s"   },
        "ʎ":    { "MUFFLE": 0, "SOUND": "yh"  },
        "x":    { "MUFFLE": 0, "SOUND": "kh"  },
        "ɲ":    { "MUFFLE": 2, "SOUND": "naw" },
        "ɣ":    { "MUFFLE": 2, "SOUND": "gr"  },
        "ʝ":    { "MUFFLE": 1, "SOUND": "zh"  },
        
        "œ̃":    { "MUFFLE": 1, "SOUND": "uh"  },
        "ɔ̃":    { "MUFFLE": 0, "SOUND": "aw"  },
        "ɑ̃":    { "MUFFLE": 0, "SOUND": "ah"  },
        "ʁ":    { "MUFFLE": 2, "SOUND": "sa"  },
        "œ":    { "MUFFLE": 1, "SOUND": "hh"  },
        "wa":   { "MUFFLE": 0, "SOUND": "wa"  },
        "ɥ":    { "MUFFLE": 1, "SOUND": "zu"  },
        "ø":    { "MUFFLE": 2, "SOUND": "euu"  },
        "ʁː":   { "MUFFLE": 2, "SOUND": "sa"  },
        "ε":    { "MUFFLE": 2, "SOUND": "eh"  },
    
        "ʏ":    { "MUFFLE": 0, "SOUND": "bh"  },
        "ʀ":    { "MUFFLE": 2, "SOUND": "rg"  },
        "õ":    { "MUFFLE": 0, "SOUND": "ou"  },
        "ẽ":    { "MUFFLE": 0, "SOUND": "en"  },
        "ĩ":    { "MUFFLE": 1, "SOUND": "eh"  },
        "ã":    { "MUFFLE": 1, "SOUND": "an"  },
        
        "ɯ":    { "MUFFLE": 1, "SOUND": "ou"  },
        "tɕ":   { "MUFFLE": 1, "SOUND": "sah"  },
        "ts":   { "MUFFLE": 1, "SOUND": "sa"  },
        "ɴ":    { "MUFFLE": 0, "SOUND": "ng"  },
        "dʑ":   { "MUFFLE": 1, "SOUND": "cha"  },
        "ɕ":    { "MUFFLE": 1, "SOUND": "sah"  },
        "ɰᵝ":   { "MUFFLE": 2, "SOUND": "mv"  },
        "ɸ":    { "MUFFLE": 0, "SOUND": "fuh"  },
        "ç":    { "MUFFLE": 1, "SOUND": "auh"  },
        "っ":   { "MUFFLE": 0, "SOUND": "っ"  },
        "ッ":   { "MUFFLE": 0, "SOUND": "ッ"  },
        "ヮ":   { "MUFFLE": 0, "SOUND": "ヮ"  },
        "ヶ":   { "MUFFLE": 0, "SOUND": "ヶ"  },
        "ゎ":   { "MUFFLE": 0, "SOUND": "ゎ"  }
    },
```