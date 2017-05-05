using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ScarletLib.BaseClasses;

namespace ScarletDAHUD
{
    class Program
    {
        private static System.Windows.Forms.NotifyIcon notifyIcon1;
        private static System.ComponentModel.IContainer components;
        private static System.Windows.Forms.ContextMenu contextMenu1;
        private static System.Windows.Forms.MenuItem menuItem1;
        
        private static void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            Application.Exit();
        }
        private static void init()
        {

            menuItem1 = new System.Windows.Forms.MenuItem();
            components = new System.ComponentModel.Container();
            

            menuItem1.Index = 0;
            menuItem1.Text = "E&xit";
            menuItem1.Click += new System.EventHandler(menuItem1_Click);

            contextMenu1 = new System.Windows.Forms.ContextMenu();
            contextMenu1.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { menuItem1 });
            // You should replace the bold icon in the sample below  
            // with an icon of your own choosing.  
            // Note the escape character used (@) when specifying the path.
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            notifyIcon1.ContextMenu = contextMenu1;
            notifyIcon1.Icon =
               new System.Drawing.Icon(System.Environment.GetFolderPath
               (System.Environment.SpecialFolder.Personal)
               + @"\Icon.ico");
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Scarlet Digital Assistant";
         
        }
       
        #region General Methods
        public static void Main()
        {
            try
            {
                init();
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAClient after init" , AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");
                ScarletLib.ScarletDAVoice.Voice.StartMorning();
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
