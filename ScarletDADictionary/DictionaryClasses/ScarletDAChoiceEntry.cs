using System;
using System.Xml.Linq;

namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Represents a class for added choices to input
    /// E.g. Input = Open, Choices = Notepad or Explorer
    /// </summary>
    public class ScarletDAChoiceEntry : ScarletDADictionaryEntry
    {
        string _text;

        ScarletDAProgramEntry _choiceProgram;

        public ScarletDAProgramEntry Program
        {
            get
            {
                return _choiceProgram;
            }

        }

        public string Text
        {
            get
            {
                return _text;
            }

            
        }

        /// <summary>
        /// Insert a text in order to make Scarlet respond to the choice
        /// Insert a program of type ScarletDAProgramEntry to make Scarlet run it
        /// </summary>
        /// <param name="Name">Name of the choice entry</param>
        /// <param name="Text">The text of the choice - phrase</param>
        /// <param name="defaultChoiceText"></param>
        public ScarletDAChoiceEntry(string Name, string Text = null ,ScarletDAProgramEntry Program = null) : base(Name, EntryType.Choice)
        {
            _text = Text;
            if (Text == null && Program == null) throw new Exception("Either text or Program must not be null");
            if (Text == null) _choiceProgram = Program;
            else _text = Text;
        }

        //Not in use at the moment
        /*
        public bool AddChoice(ScarletDADictionaryEntry newChoice, EntryType outputProgram)
        {
            if (outputProgram != EntryType.Output && outputProgram != EntryType.Program)
            {
                throw new Exception("Only Input or Program types are allowed as options");
            }
            else if (outputProgram == EntryType.Input && !(newChoice is ScarletDAInputEntry))
            {
                throw new Exception("Choice of input type must only be EntryType.Input");
            }
            else if (outputProgram == EntryType.Program && !(newChoice is ScarletDAProgramEntry))
            {

                throw new Exception("Choice of program type must only be EntryType.Program");
            }
            else
            {
                if (_choiceInput.ContainsKey(newChoice)) return false;
                _choiceInput.Add(newChoice, outputProgram);
                return true;
            }
        }*/

        public override XElement toXmlNode()
        {
            XElement tempRootElement = new XElement(XName.Get("Choice"));
            var tempText = new XAttribute(XName.Get("Name"),Name);
            tempRootElement.Add(tempText);
            XElement tempElement = null;
            if (_text == null)
            {
                tempElement = new XElement(XName.Get("Text"));
                tempElement.SetValue(_text);
            }
            else tempElement = Program.toXmlNode();
            tempRootElement.Add(tempElement);
            return tempRootElement;
        }
    }
}
