using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletDADictionary.DictionaryClasses
{
    class ScarletDAChoiceEntry : ScarletDADictionaryEntry
    {
        string _text;
        Dictionary<ScarletDADictionaryEntry, EntryType> ChoiceInput;
        public ScarletDAChoiceEntry(string Name, string Text, Dictionary<ScarletDADictionaryEntry, EntryType> defaultChoiceInput = null) : base(Name, EntryType.Choice)
        {
            _text = Text;
            ChoiceInput = defaultChoiceInput==null? new Dictionary<ScarletDADictionaryEntry, EntryType>():defaultChoiceInput;
        }
    }
}
