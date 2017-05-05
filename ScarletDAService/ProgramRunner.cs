using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ScarletDAService
{
    public partial class ProgramRunner : ServiceBase
    {
        List<ScarletLib.BaseClasses.Program> programs;
        public ProgramRunner()
        {
            programs = new List<ScarletLib.BaseClasses.Program>();
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {

            ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAService Started!", AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt");
            ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAService client mode Started!", AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt");
            ScarletLib.BaseClasses.ScarletDAClient c = new ScarletLib.BaseClasses.ScarletDAClient();
            c.ClientRun(null);
            ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAService got thread!", AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt");

            ScarletLib.BaseClasses.ScarletDAServer.StartServer();
        }

        protected override void OnStop()
        {
            ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAService Stopped!",AppDomain.CurrentDomain.BaseDirectory+"ServiceLog.txt");
        }
    }
}
