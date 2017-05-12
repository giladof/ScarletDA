using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarletLib;

namespace ScarletDATest
{
    class Program
    {
        static void Main(string[] args)
        {
            ScarletLib.BaseClasses.ScarletDAProgram notepad = new ScarletLib.BaseClasses.ScarletDAProgram("Notepad", "notepad.exe", null);
            ScarletLib.BaseClasses.ScarletDAProgram ping = new ScarletLib.BaseClasses.ScarletDAProgram("Ping", "ping.exe", null);
            ping.AddArgument("127.0.0.1");
            ping.AddArgument("-t");
            Task<string> result = ping.RunmeAsync();
            ping.ProgramChanged += ping_ProgramChanged;
            while (!result.IsCompleted)
            {
                Console.WriteLine("Current Status: {0}", ping.ProgramState);
                System.Threading.Thread.Sleep(3000);
            }
            Console.WriteLine("Current Status: {0}", ping.ProgramState);
        }

        private static void ping_ProgramChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Raised event that program has changed: current value is {0}", (sender as ScarletLib.BaseClasses.ScarletDAProgram).ProgramState);
        }
    }
}
