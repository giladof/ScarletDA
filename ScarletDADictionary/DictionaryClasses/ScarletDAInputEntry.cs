using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Represents phrase(s) for the speech recognition
    /// An input entry could contain an output (for Scarlet to respond), a program (for Scarlet to run) or other choices that contain further options
    /// Either insert choices or output/program, not both
    /// </summary>
    public class ScarletDAInputEntry : ScarletDADictionaryEntry
    {
        private ScarletDAProgramEntry _choiceProgram;
        string[] _text;
        private ScarletDAChoiceEntry[] _choices;


        public ScarletDAInputEntry(string Name,string[] InputText, ScarletDAChoiceEntry[] Choices = null, string Outputtext = null, ScarletDAProgramEntry Program = null) : base(Name, EntryType.Input)
        {
            _text = InputText;
            if (Choices != null)
            {
                if (InputText.Length > 1) throw new Exception("Input has to be single if using choices");
                _choices = Choices;
            }
            else
            {
                if (Outputtext == null && Program == null) throw new Exception("Either text or Program must not be null");
                if (Outputtext == null) _choiceProgram = Program;
            }
            
        }

        public ScarletDAProgramEntry Program
        {
            get
            {
                return _choiceProgram;
            }

           
        }

        /// <summary>
        /// Each option of the Text array represents an option that generates the same response
        /// </summary>
        public string[] Text
        {
            get
            {
                return _text;
            }
        }

        public ScarletDAChoiceEntry[] Choices
        {
            get
            {
                return _choices;
            }
        }

        public override XElement toXmlNode()
        {
            XElement tempRootElement = new XElement(XName.Get("Input"));
            tempRootElement.Add(new XAttribute(XName.Get("Name"),Name));
            foreach (var str in _text)
            {
                tempRootElement.Add(new XElement(XName.Get("Text"), str));
            }
            if (Choices != null)
            {
                var tempChoicesRoot = new XElement(XName.Get("Choices"));
                foreach (var Choice in Choices)
                {
                    tempChoicesRoot.Add(Choice.toXmlNode());
                }
                tempRootElement.Add(tempChoicesRoot);
            }

            else
            {
                XElement tempElement = null;
                if (_text == null)
                {
                    tempElement = new XElement(XName.Get("Text"));
                    tempElement.SetValue(_text);
                }
                else tempElement = Program.toXmlNode();
                tempRootElement.Add(tempElement);
            }
            return tempRootElement;
        }
    }
}
