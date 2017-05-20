using ScarletDADictionary.Interfaces;
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
        ScarletDAOutputEntry _outputtext;

        public ScarletDAInputEntry(string Name,string[] InputText, ScarletDAChoiceEntry[] Choices = null, ScarletDAOutputEntry Outputtext = null, ScarletDAProgramEntry Program = null) : base(Name, EntryType.Input)
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
                else _outputtext = Outputtext;
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

        public ScarletDAOutputEntry Outputtext
        {
            get
            {
                return _outputtext;
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
                if (Outputtext != null)tempElement.Add(_outputtext.toXmlNode());
                else tempElement = Program.toXmlNode();
                tempRootElement.Add(tempElement);
            }
            return tempRootElement;
        }

        public static new  ScarletDAInputEntry fromXmlNode(XElement node)
        {
            var strName = node.Attribute(XName.Get("Name")).Value;
            var strTextElem = node.Elements(XName.Get("Text"));
            List<ScarletDAChoiceEntry> listChoices = null;
            ScarletDAProgramEntry listPrograms = null;
            ScarletDAOutputEntry listOutput = null;
            List<string> textArr = new List<string>();
            foreach (var el in strTextElem)
            {
                textArr.Add(el.Value);
            }
            var elChoices = node.Element("Choices");
            
            if (elChoices != null)
            {
                listChoices = new List<ScarletDAChoiceEntry>();
                foreach (var choice in elChoices.Descendants())
                {
                    listChoices.Add(ScarletDAChoiceEntry.fromXmlNode(choice));
                }
            }
            return new ScarletDAInputEntry(strName, textArr.ToArray(), listChoices.ToArray(), listOutput, listPrograms);
            
        }

        
    }
}

