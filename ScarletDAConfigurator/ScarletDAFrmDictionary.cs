using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletDAConfigurator
{
    public class ScarletDAFrmDictionary
    {
        public ScarletDADictionary.DictionaryClasses.EntryType EntryType { get; set; }
        public string Name { get; set; }
        public ScarletDADictionary.DictionaryClasses.ScarletDADictionaryEntry Value { get; set; }
    }
}
