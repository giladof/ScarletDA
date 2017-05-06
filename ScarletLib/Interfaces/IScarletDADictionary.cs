using ScarletLib.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletLib.Interfaces
{
    interface IScarletDADictionary
    {
        bool AddProgramToDictonary( ScarletDAProgram programInfo);
        bool RemoveProgramFromDictionary(string programName);
        ScarletDAProgram[] ListAllPrograms();
        ScarletDAProgram GetProgram(string ProgramName);
        Task<bool> RunProgram(string ProgramName);
    }
}
