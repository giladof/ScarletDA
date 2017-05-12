using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarletLib.BaseClasses;
namespace ScarletDADictionary.DictionaryClasses
{
    /// <summary>
    /// Represents a program for Scarlet to run
    /// </summary>
    public class ScarletDAProgramEntry : ScarletDADictionaryEntry
    {
        ScarletDAProgram _action;
        string _text;
        int _id;

        public ScarletDAProgramEntry(string Name,string Text,ScarletDAProgram Action) : base(Name, EntryType.Program)
        {
            _id = -1;
            _text = Text;
            _action = Action;
            _action.ProgramChanged += _action_ProgramChanged;
        }

        private void _action_ProgramChanged(object sender, EventArgs e)
        {
            _id = (sender as ScarletDAProgram).ProcID;
        }

        public string Text
        {
            get
            {
                return _text;
            }
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
