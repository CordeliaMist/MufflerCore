using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MufflerCore;
public class MuffleObject : IMuffleObject
{
    public string MuffleSourceItem { get; set; } = "";        // Name of the muffling object
    public Dictionary<string, int> MuffleStrengthOfPhoneme { get; set; } = new Dictionary<string, int>(); // Stores muffle strength for phoneme.
    public Dictionary<string, string> IpaSymbolSound { get; set; } = new Dictionary<string, string>(); // Stores the IPA symbol sound.

    public MuffleObject() { }

    public void AddInfo(string name, Dictionary<string, int> muffleStrOnPhoneme, Dictionary<string, string> ipaSymbolSound)
    {
        MuffleSourceItem = name;
        MuffleStrengthOfPhoneme = muffleStrOnPhoneme ?? throw new ArgumentNullException(nameof(muffleStrOnPhoneme));
        IpaSymbolSound = ipaSymbolSound ?? throw new ArgumentNullException(nameof(ipaSymbolSound));
    }
}