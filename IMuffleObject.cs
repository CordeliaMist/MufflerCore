using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MufflerCore;
public interface IMuffleObject
{
    public string MuffleSourceItem { get; set; } // Name of the muffling object
    public Dictionary<string, int> MuffleStrengthOfPhoneme { get; set; } // Stores muffle strength for phoneme.
    public Dictionary<string, string> IpaSymbolSound { get; set; } // Stores the IPA symbol sound.

    public void AddInfo(string name, Dictionary<string, int> muffleStrOnPhoneme, Dictionary<string, string> ipaSymbolSound);
}