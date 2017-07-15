using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ScarletLib.BaseClasses;
using System.Threading;
using System.Security.Permissions;
using System.Speech.Recognition;

namespace ScarletDAHUD
{
    public class Program
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
            components.Remove(ScarletDANotifyIcon);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
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
            menuExit.Index = 0;
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
            menuListen.Index = 2;
            menuListen.Text = "S&top listening";
            menuListen.Click += new System.EventHandler(menuListenHandle_Click);
            //
            //mainContextMenu
            mainContextMenu = new System.Windows.Forms.ContextMenu();
            mainContextMenu.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { menuHook, menuListen,menuExit });
            //
            //ScarletDANotifyIcon
            ScarletDANotifyIcon = new System.Windows.Forms.NotifyIcon(components);
            ScarletDANotifyIcon.ContextMenu = mainContextMenu;
            ScarletDANotifyIcon.Icon = Properties.Resources.daIco;
            ScarletDANotifyIcon.Visible = true;
            ScarletDANotifyIcon.Text = "Scarlet Digital Assistant";
            //
            //Hook keyboard
            menuHookHandle_Click(null,null);
        }





        #region General Methods
       
        public static void ListenSpeak()
        {
            while (_speak)
            {
                ScarletDASpeechListener.Listener.Listen();
                if (ScarletDASpeechListener.Listener.isInit)
                {
                    ScarletDAVoice.Voice.SpeakPhrase("Hey, How can I help?");
                    ScarletDASpeechListener.Listener.Listen();
                }
            }
        }

        public static void Main()
        {
            try
            {
                init();
               ScarletLogger.LogMessage("Starting Scarlet Digital Assistant" , AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");
                ScarletDAVoice.Voice.SpeakPhrase("Hello!");
                ScarletDAKeyboard.KeyboardExecuted += ScarletDAKeyboard_KeyboardExecuted;
                ScarletDASpeechListener.Listener.GoToSleep += Listener_GoToSleep;
                SpeakListenRunner = new Thread(ListenSpeak);
                SpeakListenRunner.Start();
                Application.Run();
               
            }
            catch (Exception e)
            {
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAClient Error init " + e.Message, AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");
                foreach (RecognizerInfo ri in SpeechRecognitionEngine.InstalledRecognizers())
                {
                    System.Diagnostics.Debug.WriteLine(ri.Culture.Name);
                }
            }
        }

        private static void Listener_GoToSleep(object sender, EventArgs e)
        {
            menuListenHandle_Click(null, null);
        }

        private static void ScarletDAKeyboard_KeyboardExecuted(object sender, ScarletLib.BaseClasses.ScarletDAKeyboard.KeyEventArgs e)
        {
            if (e.SystemKey)
            {
                switch (e.KeyPressed)
                {
                    case Keys.S:
                        menuListenHandle_Click(null, null);
                        break;
                    case Keys.Q:
                        menuExit_Click(null, null);
                        break;
                }
            }
        }
        #endregion
    }
}
