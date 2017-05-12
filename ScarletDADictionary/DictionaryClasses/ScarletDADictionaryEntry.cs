using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Enum of the type of entry
    /// </summary>
    public enum EntryType { Input, Output, Choice, Program }

    /// <summary>
    /// Represents the base classes for Dictionary entries
    /// </summary>
    public class ScarletDADictionaryEntry
    {
        private string _name;
        public string Name { get { return _name; } }

        EntryType _entryType;
        public EntryType Type { get { return _entryType; } }
        public ScarletDADictionaryEntry(string Name, EntryType type)
        {
            Name = _name;
            _entryType = type;
        }

    }

}
