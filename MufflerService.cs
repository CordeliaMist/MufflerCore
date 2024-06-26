using MufflerCore.LanguageToIpa;
using MufflerCore.PhonemeLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MufflerCore;
public class MufflerService
{
    /// <summary>
    /// This contains the json parsed data for the translation information of various MuffleObjects.
    /// </summary>
    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> MuffleObjectsData;
    public List<MuffleObject> ActiveMuffleObjects;

    public MufflerService(Dictionary<string, Dictionary<string, Dictionary<string, string>>> MuffleObjDataSource, string languageCode)
    {
        MuffleObjectsData = MuffleObjDataSource;
        ActiveMuffleObjects = new List<MuffleObject>();

        // create our gag listings
        CreateGags(languageCode);
    }

    private void CreateGags(string languageCode)
    {
        List<string> masterList;
        switch (languageCode)
        {
            case "IPA_UK": masterList = PhonemeMasterLists.MasterListEN_UK; break;
            case "IPA_US": masterList = PhonemeMasterLists.MasterListEN_US; break;
            case "IPA_SPAIN": masterList = PhonemeMasterLists.MasterListSP_SPAIN; break;
            case "IPA_MEXICO": masterList = PhonemeMasterLists.MasterListSP_MEXICO; break;
            case "IPA_FRENCH": masterList = PhonemeMasterLists.MasterListFR_FRENCH; break;
            case "IPA_QUEBEC": masterList = PhonemeMasterLists.MasterListFR_QUEBEC; break;
            case "IPA_JAPAN": masterList = PhonemeMasterLists.MasterListJP; break;
            default: throw new Exception("Invalid language");
        }

        foreach (var gagEntry in MuffleObjectsData)
        {
            var gagName = gagEntry.Key;
            var muffleStrOnPhoneme = new Dictionary<string, int>();
            var ipaSymbolSound = new Dictionary<string, string>();
            foreach (var phonemeEntry in gagEntry.Value)
            {
                var phoneme = phonemeEntry.Key;
                var properties = phonemeEntry.Value;
                muffleStrOnPhoneme[phoneme] = int.Parse(properties["MUFFLE"]);
                ipaSymbolSound[phoneme] = properties["SOUND"];
            }
            var gag = new MuffleObject();
            gag.AddInfo(gagName, muffleStrOnPhoneme, ipaSymbolSound);
            ActiveMuffleObjects.Add(gag);
        }
    }
}
