using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Enum of the type of entry
    /// </summary>
    public enum EntryType { Input, Output, Choice, Program }

    /// <summary>
    /// Represents the base classes for Dictionary entries
    /// </summary>
    public abstract class ScarletDADictionaryEntry
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

        public abstract XElement toXmlNode();

    }

}
