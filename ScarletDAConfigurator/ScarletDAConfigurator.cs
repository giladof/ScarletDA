using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ScarletDADictionary.DictionaryClasses;
namespace ScarletDAConfigurator
{
    public  class ScarletDAConfigurator 
    {
        XElement _document;
        bool _docExists;
        List<ScarletDAInputEntry> _Input;

        string _xmlPath = AppDomain.CurrentDomain.BaseDirectory + "ScarletDAConfiguration.xml";
        public bool ConfigurationExists
        {
            get
            {
                return _docExists;
            }

            
        }

        public ScarletDAInputEntry[] Input
        {
            get
            {
                if (_Input == null) return new ScarletDAInputEntry[0];
                return _Input.ToArray() ;
            }

           
        }

        public ScarletDAConfigurator()
        {
            if (File.Exists(_xmlPath))
            {
                
                try
                {
                    _document = XElement.Load(_xmlPath);
                    _docExists = true;
                    ParseDocument(_document);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ScarletDA Configuration parsing error: "+e.Message);
                    _document = new XElement(XName.Get("Input"));
                    _docExists = false;
                }
            }
            else
            {
                _document = new XElement(XName.Get("Input"));
                _document.Save(_xmlPath);
                _docExists = false;
            }
        }

        private void ParseDocument(XElement _document)
        {
            _Input = new List<ScarletDAInputEntry>();
            foreach (var vInput in _document.Descendants())
            {
                _Input.Add(ScarletDAInputEntry.fromXmlNode(vInput));
            }
        }
        public bool SaveDocument()
        {
            try
            {

                _document.Save(_xmlPath);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ScarletDA Configuration saving error: " + e.Message);
                return false;
            }
        }
    }
}
