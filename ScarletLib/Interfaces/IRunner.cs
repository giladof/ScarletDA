using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletLib.Interfaces
{
    interface IRunner
    {

        
        /// <summary>
        /// Implement Runme() to run a certain command via Windows-Run
        /// </summary>
        string Runme();

        /// <summary>
        /// Implement Runme() to run a certain command via Windows-Run as async
        /// </summary>
        Task<string> RunmeAsync();
        
    }
}
