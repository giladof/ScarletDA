using ScarletDADictionary.DictionaryClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScarletDAConfigurator
{
    public partial class frmMain : Form
    {
        ScarletDAConfigurator config;
        
        public frmMain()
        {
            config = new ScarletDAConfigurator();
            InitializeComponent();
            treeItems.NodeMouseClick += TreeItems_NodeMouseClick;
            
        }

        private void TreeItems_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name == "Dictionary" || e.Node.Name.StartsWith("No entries. "))
                return;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msgBoxResult = MessageBox.Show("Some changes were not saved.\nAre you sure you want to quit without saving?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            switch (msgBoxResult)
            {
                case DialogResult.Yes:
                    Application.Exit();
                    break;
                case DialogResult.No:
                    SaveDocument();
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }

        private void SaveDocument()
        {
            config.SaveDocument();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config.SaveDocument();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            int count = 0;
            treeItems.Nodes.Add("Dictionary");
            foreach (ScarletDAInputEntry entry in config.Input)
            {
                treeItems.Nodes[0].Nodes.Add(entry.Name);
                count++;
            }
            if (count == 0)
            {
                treeItems.Nodes[0].Nodes.Add("No entries. Click to add");
            }
        }
    }
}
