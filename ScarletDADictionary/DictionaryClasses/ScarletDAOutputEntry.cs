using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Represents text for scarlet to speak in text to speech
    /// </summary>
    public class ScarletDAOutputEntry : ScarletDADictionaryEntry
    {
        string _text;

        public ScarletDAOutputEntry(string Name,string Text) : base(Name, EntryType.Output)
        {
            _text = Text;
        }
        
        public string Text
        {
            get
            {
                return _text;
            }    
        }
    }
}
