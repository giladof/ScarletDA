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
            ScarletLib.BaseClasses.Program notepad = new ScarletLib.BaseClasses.Program("Notepad", "notepad.exe", null);
            Task<string> result = notepad.RunmeAsync();
            notepad.ProgramChanged += Notepad_ProgramChanged;
            while (!result.IsCompleted)
            {
                Console.WriteLine("Current Status: {0}",notepad.ProgramState);
                System.Threading.Thread.Sleep(3000);
            }
            Console.WriteLine("Current Status: {0}", notepad.ProgramState);
        }

        private static void Notepad_ProgramChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Raised event that program has changed: current value is {0}", (sender as ScarletLib.BaseClasses.Program).ProgramState);
        }
    }
}
