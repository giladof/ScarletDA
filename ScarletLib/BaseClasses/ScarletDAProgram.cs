﻿using ScarletLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScarletLib.BaseClasses
{
    /// <summary>
    /// This class allows running a program via command. A new process will be started.
    /// This process will be monitored by this class.
    /// </summary>
    public class ScarletDAProgram:IRunner
    {
        #region Constructors
        /// <summary>
        /// This class is assosiated with the Scarlet Dictionary, the name should correspond with the action for the voice / keyboard command.
        /// </summary>
        /// <param name="name">A friendly name for the program</param>
        /// <param name="command">The command that needs to run (e.g. notepad.exe)</param>
        /// <param name="workingDirectory">Allows null for no directory (local directory of the service)</param>
        public ScarletDAProgram(string name, string command, string workingDirectory)
        {
            this.Name = name;
            this.Command = command;
            this.WorkingDirectory = workingDirectory;
        }
        #endregion

        #region Events
        public event EventHandler ProgramChanged;
        #endregion

        #region Members
        Process proc;
        string _name;
        string _command;
        Dictionary<string, string> _arguments;
        string _workingDirectory;
        int _procID;
        #endregion

        #region Properties
        public string Name {get { return (_name != null ? _name:""); } set
            {
                if (value=="" || value==null )
                    throw new Exception("Name must not be null!");
                _name = value;
            }
        }
        public string Command { get { return _command; }  set {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Command must not be null!");
                _command = value;
            } }
        public Dictionary<string,string> Arguments { get
            {
                
                if (_arguments == null)
                {
                    return _arguments = new Dictionary<string, string>();
                }
                return _arguments;
            } private set { } }
        public State ProgramState { get
            {
                if (proc == null)
                {
                    return State.NotStarted;
                }
                else
                {
                    try
                    {
                        Process.GetProcessById(proc.Id);
                        return State.Running;
                    }
                    catch (Exception)
                    {
                        return State.Stopped;
                    }
                }
            }
           

        }
        public string WorkingDirectory { get
            {
                return _workingDirectory;
            } set
            {
                if (string.IsNullOrWhiteSpace(value)) _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                else _workingDirectory = value;
            } }
        public int ProcID
        {
            get
            {
               
                return _procID;
            }
            private set
            {
                _procID = value;
            }
        }
        #endregion

        #region Enums
        public enum State { Running, Stopped, Error,NotStarted };
        #endregion

        #region Methods
        public void AddArgument(string argValue="", string argSwitch="")
        {
            if (argValue == null && argSwitch == null) throw new Exception("One parameter must not be null!");
            Arguments.Add(argSwitch, argValue);
        }
        public void RemoveArguments()
        {
            Arguments.Clear();

        }

        protected virtual void OnProgramChanged(EventArgs e)
        {
            EventHandler handler = ProgramChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private IntPtr handle;
        public string Runme()
        {
            proc = new Process();
            proc.StartInfo= new ProcessStartInfo()
            {
                WorkingDirectory = this.WorkingDirectory,
                FileName = this.Command,
                Arguments = this.GetArgs(),
                RedirectStandardOutput = true,
                UseShellExecute=false
            };
            Console.WriteLine(this.Command + this.GetArgs());
            
            proc.Start();
            ProcID = proc.Id;
            handle = proc.MainWindowHandle;
            SetForegroundWindow(handle);
            OnProgramChanged(EventArgs.Empty); 
            while (!proc.HasExited)
            {
                proc.WaitForExit();
            }
            OnProgramChanged(EventArgs.Empty);
            if (proc.ExitCode != 0) throw new Exception(String.Format("Error: ExitCode: {0}, Error is:{1}", proc.ExitCode, proc.StandardError.ReadToEnd()));
            return proc.StandardOutput.ReadToEnd();
        }
        

        private string GetArgs()
        {
            StringBuilder b = new StringBuilder();
            foreach (var arg in this.Arguments)
            {
                if (arg.Key == null) b.Append(String.Format(" {0} ", arg.Value));
                else if (arg.Value == "") b.Append(String.Format(" {0} ", arg.Key));
                else b.Append(String.Format(" {0} {1}", arg.Key, arg.Value));
            }
            return b.ToString();
        }

       async public Task<string> RunmeAsync()
        {
            return await Task.Run(() => { return Runme(); });
        }
        #endregion
    }
}
