using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ScarletLib.BaseClasses;
namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Represents a program for Scarlet to run
    /// </summary>
    public class ScarletDAProgramEntry : ScarletDADictionaryEntry
    {
        ScarletDAProgram _action;

        int _id;

        public ScarletDAProgramEntry(string Name,ScarletDAProgram Action) : base(Name, EntryType.Program)
        {
            _id = -1;
            
            _action = Action;
            _action.ProgramChanged += _action_ProgramChanged;
        }

        private void _action_ProgramChanged(object sender, EventArgs e)
        {

            var prog = (sender as ScarletDAProgram);
            if (prog.ProgramState == ScarletDAProgram.State.Running) _id = prog.ProcID;
            else _id = -1;
        }

        public override XElement toXmlNode()
        {
            XElement tempRootElement = new XElement(XName.Get("Program"));
            var tempText = new XAttribute(XName.Get("Name"), Name);
            tempRootElement.Add(tempText);
            XElement tempCommand = new XElement(XName.Get("Command"), _action.Command);
            if (_action.Arguments.Count != 0)
            {
                XElement tempArguments = new XElement(XName.Get("Arguments"));
               
                foreach (var arg in _action.Arguments)
                {
                    tempArguments.Add( new XElement(XName.Get("Argument"), arg));

                }
                tempRootElement.Add(tempArguments);
            }
            if (_action.WorkingDirectory != null)
            {
                tempRootElement.Add(new XElement(XName.Get("WorkingDirectory"), _action.WorkingDirectory));
            }
            tempRootElement.Add(tempCommand);
            return tempRootElement;
        }

        public ScarletDAProgram Action
        {
            get
            {
                return _action;
            }
        }

        public int Id
        {
            get
            {
                if (_id == -1)
                    throw new Exception("Program has not started");
                else return _id; 
            }

           
        }
    }
}
