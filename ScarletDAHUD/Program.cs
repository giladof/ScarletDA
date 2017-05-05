using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ScarletLib.BaseClasses;
using System.Threading;
using System.Security.Permissions;

namespace ScarletDAHUD
{
    class Program
    {
        private static System.Windows.Forms.NotifyIcon ScarletDANotifyIcon;
        private static System.ComponentModel.IContainer components;
        private static System.Windows.Forms.ContextMenu mainContextMenu;
        private static System.Windows.Forms.MenuItem menuExit;
        private static System.Windows.Forms.MenuItem menuHook;
        private static System.Windows.Forms.MenuItem menuListen;
        private static bool _hooked = false;
        private static bool _listen = false;
        private static Thread SpeakListenRunner;
        private static bool _speak=true;
        #region Event Handlers
        private static void menuExit_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            Application.Exit();
        }
        private static void menuListenHandle_Click(object sender, EventArgs e)
        {
            if (!_listen)
            {
                _listen = true;
                ScarletLib.BaseClasses.ScarletDASpeechListener.Listener.StartListening();
                menuListen.Text = "S&top listening";
                _speak = true;
                if (SpeakListenRunner.ThreadState == ThreadState.Stopped)
                {
                    SpeakListenRunner = new Thread(ListenSpeak);
                    SpeakListenRunner.Start();

                }
            }
            else
            {
                _listen = false;
                ScarletLib.BaseClasses.ScarletDASpeechListener.Listener.StopListening();
                menuListen.Text = "Lis&ten";
                _speak = false;
            }
        }
        private static void menuHookHandle_Click(object sender, EventArgs e)
        {
            if (!_hooked)
            {
                _hooked = true;
                ScarletDAKeyboard.StartHook();
                menuHook.Text = "U&nhook";
            }
            else
            {
                _hooked = false;
                ScarletDAKeyboard.StopHook();
                menuHook.Text = "H&ook";
            }
        }
        #endregion
        private static void init()
        {
            //Init
            
            components = new System.ComponentModel.Container();
            
            //menuExit
            menuExit = new System.Windows.Forms.MenuItem();
            menuExit.Index = 2;
            menuExit.Text = "E&xit";
            menuExit.Click += new System.EventHandler(menuExit_Click);
            //
            //menuHook
            menuHook = new MenuItem();
            menuHook.Index = 1;
            menuHook.Text = "H&ook";
            menuHook.Click += new System.EventHandler(menuHookHandle_Click);
            //
            //menuListen
            menuListen = new MenuItem();
            menuListen.Index = 0;
            menuListen.Text = "S&top listening";
            menuListen.Click += new System.EventHandler(menuListenHandle_Click);
            //
            //mainContextMenu
            mainContextMenu = new System.Windows.Forms.ContextMenu();
            mainContextMenu.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { menuExit,menuHook, menuListen });
            //
            //ScarletDANotifyIcon
            ScarletDANotifyIcon = new System.Windows.Forms.NotifyIcon(components);
            ScarletDANotifyIcon.ContextMenu = mainContextMenu;
            ScarletDANotifyIcon.Icon =
               new System.Drawing.Icon(System.Environment.GetFolderPath
               (System.Environment.SpecialFolder.Personal)
               + @"\Icon.ico");
            ScarletDANotifyIcon.Visible = true;
            ScarletDANotifyIcon.Text = "Scarlet Digital Assistant";
            //
        }





        #region General Methods
       
        public static void ListenSpeak()
        {
            while (_speak)
            {
                ScarletLib.ScarletDAVoice.Voice.SpeakPhrase("Which program would you like me to open?");
                ScarletDASpeechListener.Listener.Listen();
            }
        }

        public static void Main()
        {
            try
            {
                init();
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAClient after init" , AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");
                ScarletLib.ScarletDAVoice.Voice.SpeakPhrase("Good Morning Sir! How did you sleep?");
                SpeakListenRunner = new Thread(ListenSpeak);
                SpeakListenRunner.Start();
                Application.Run();
               
            }
            catch (Exception e)
            {
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAClient Error init " + e.Message, AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");

            }
        }
        #endregion
    }
}
