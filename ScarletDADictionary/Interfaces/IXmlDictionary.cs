using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScarletDADictionary.Interfaces
{
    interface IXmlDictionary
    {
        XElement toXmlNode();
        DictionaryClasses.ScarletDADictionaryEntry fromXmlNode(XElement node);
    }
}
