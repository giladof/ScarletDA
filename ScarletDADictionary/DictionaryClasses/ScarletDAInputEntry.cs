using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Represents phrase(s) for the speech recognition
    /// </summary>
    public class ScarletDAInputEntry : ScarletDADictionaryEntry
    {
        string[] _text;

        public ScarletDAInputEntry(string Name,string[] Text) : base(Name, EntryType.Input)
        {
            _text = Text;
        }
        public string[] Text
        {
            get
            {
                return _text;
            }
        }
    }
}
