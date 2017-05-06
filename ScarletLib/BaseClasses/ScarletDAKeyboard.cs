using System;

using System.Diagnostics;

using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScarletLib.BaseClasses
{
   public class ScarletDAKeyboard
    {
        #region Private Members
        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private static LowLevelKeyboardProc _proc = HookCallback;

        private static IntPtr _hookID = IntPtr.Zero;
        public static uint ClientThread;

          #endregion

        #region General Methods
        public static void StartHook()
        {
               _hookID = SetHook(_proc);
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAHook intercepting keyboard!", AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");
        }
        public static void StopHook()
        {
            UnhookWindowsHookEx(_hookID);
            ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAHook stopped intercepting keyboard!", AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");
        }
        #endregion
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        #region Keyboard Hooks
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private delegate IntPtr LowLevelKeyboardProc(

            int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        public static event EventHandler<KeyEventArgs> KeyboardExecuted;
        protected static void OnKeyboardExecuted(KeyEventArgs e)
        {
            EventHandler<KeyEventArgs> handler = KeyboardExecuted;
            if (handler != null)
            {
                handler(null, e);
            }
        }
        private static IntPtr HookCallback(

            int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam)

        {
            if (nCode >= 0 && wParam == (IntPtr)WM_SYSKEYDOWN)
            {


                    ScarletLib.BaseClasses.ScarletLogger.LogMessage(String.Format("ScarletDAHud intercepted {0}, performing action!", (Keys)lParam.vkCode), AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");
                    var KeyArgs = new KeyEventArgs();
                    KeyArgs.SystemKey = (wParam == (IntPtr)WM_SYSKEYDOWN);
                    KeyArgs.KeyPressed = (Keys)lParam.vkCode;
                    OnKeyboardExecuted(KeyArgs);

                
            }
            return CallNextHookEx(_hookID, nCode, wParam,ref lParam);
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,

            IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public class KeyEventArgs : EventArgs
        {
            public bool SystemKey { get; set; }
            public Keys KeyPressed { get; set; }
        }
    }
}
