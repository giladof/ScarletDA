using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarletLib.Interfaces;
namespace ScarletLib.BaseClasses
{
    class ScarletDADictionaryBase : IScarletDADictionary
    {
        Dictionary<int, ScarletDAProgram> Dic;
        public ScarletDADictionaryBase()
        {
            Dic = new Dictionary<int, ScarletDAProgram>();
        }
        public bool AddProgramToDictonary( ScarletDAProgram programInfo)
        {
            if (Dic.Where(val => val.Value.Name == programInfo.Name).ToList().Count != 0) return false;
            try
            {
                Dic.Add(Dic.Count, programInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public ScarletDAProgram GetProgram(string ProgramName)
        {
            List<ScarletDAProgram> list = Dic.Where(val => val.Value.Name == ProgramName).Select(val => val.Value).ToList();
            if (list.Count == 0) return null;
            return list[0];

        }
        public ScarletDAProgram[] ListAllPrograms()
        {
            List<ScarletDAProgram> programs = new List<ScarletDAProgram>();
            foreach (var p in Dic)
            {
                programs.Add(p.Value);
            }
            return programs.ToArray();
        }

        public bool RemoveProgramFromDictionary(string ProgramName)
        {
            if (Dic.Where(val => val.Value.Name == ProgramName).ToList().Count == 0) return false;
            Dictionary<int, ScarletDAProgram> DicTemp = new Dictionary<int, ScarletDAProgram>();
            int i = 0;
            foreach (var inst in Dic)
            {
                if (inst.Value.Name != ProgramName)
                {
                    DicTemp.Add(i++, inst.Value);
                }
                    
            }
            Dic = DicTemp;
            return true;
        }



        public async Task<bool> RunProgram(string ProgramName)
        {
            
            List<ScarletDAProgram> list = Dic.Where(val => val.Value.Name == ProgramName).Select(val=>val.Value).ToList();
            if (list.Count == 0) return false;
             await list[0].RunmeAsync();
            return true;
        }
    }
}
