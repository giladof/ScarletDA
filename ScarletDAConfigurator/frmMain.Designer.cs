namespace ScarletDAConfigurator
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spPanMain = new System.Windows.Forms.SplitContainer();
            this.treeItems = new System.Windows.Forms.TreeView();
            this.tabDictionaryInfo = new System.Windows.Forms.TabControl();
            this.tabInput = new System.Windows.Forms.TabPage();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.tabChoice = new System.Windows.Forms.TabPage();
            this.tabProgram = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spPanMain)).BeginInit();
            this.spPanMain.Panel1.SuspendLayout();
            this.spPanMain.Panel2.SuspendLayout();
            this.spPanMain.SuspendLayout();
            this.tabDictionaryInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(439, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "F&ile";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.saveToolStripMenuItem.Text = "S&ave";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // spPanMain
            // 
            this.spPanMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spPanMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spPanMain.Location = new System.Drawing.Point(0, 28);
            this.spPanMain.Name = "spPanMain";
            // 
            // spPanMain.Panel1
            // 
            this.spPanMain.Panel1.Controls.Add(this.treeItems);
            // 
            // spPanMain.Panel2
            // 
            this.spPanMain.Panel2.Controls.Add(this.tabDictionaryInfo);
            this.spPanMain.Size = new System.Drawing.Size(439, 426);
            this.spPanMain.SplitterDistance = 150;
            this.spPanMain.TabIndex = 2;
            // 
            // treeItems
            // 
            this.treeItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeItems.Location = new System.Drawing.Point(0, 0);
            this.treeItems.Name = "treeItems";
            this.treeItems.Size = new System.Drawing.Size(148, 424);
            this.treeItems.TabIndex = 0;
            // 
            // tabDictionaryInfo
            // 
            this.tabDictionaryInfo.Controls.Add(this.tabInput);
            this.tabDictionaryInfo.Controls.Add(this.tabOutput);
            this.tabDictionaryInfo.Controls.Add(this.tabChoice);
            this.tabDictionaryInfo.Controls.Add(this.tabProgram);
            this.tabDictionaryInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDictionaryInfo.Location = new System.Drawing.Point(0, 0);
            this.tabDictionaryInfo.Name = "tabDictionaryInfo";
            this.tabDictionaryInfo.SelectedIndex = 0;
            this.tabDictionaryInfo.Size = new System.Drawing.Size(283, 424);
            this.tabDictionaryInfo.TabIndex = 0;
            // 
            // tabInput
            // 
            this.tabInput.Location = new System.Drawing.Point(4, 25);
            this.tabInput.Name = "tabInput";
            this.tabInput.Padding = new System.Windows.Forms.Padding(3);
            this.tabInput.Size = new System.Drawing.Size(275, 395);
            this.tabInput.TabIndex = 0;
            this.tabInput.Text = "Input";
            this.tabInput.UseVisualStyleBackColor = true;
            // 
            // tabOutput
            // 
            this.tabOutput.Location = new System.Drawing.Point(4, 25);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabOutput.Size = new System.Drawing.Size(275, 395);
            this.tabOutput.TabIndex = 1;
            this.tabOutput.Text = "Output";
            this.tabOutput.UseVisualStyleBackColor = true;
            // 
            // tabChoice
            // 
            this.tabChoice.Location = new System.Drawing.Point(4, 25);
            this.tabChoice.Name = "tabChoice";
            this.tabChoice.Size = new System.Drawing.Size(275, 395);
            this.tabChoice.TabIndex = 2;
            this.tabChoice.Text = "Choice";
            this.tabChoice.UseVisualStyleBackColor = true;
            // 
            // tabProgram
            // 
            this.tabProgram.Location = new System.Drawing.Point(4, 25);
            this.tabProgram.Name = "tabProgram";
            this.tabProgram.Size = new System.Drawing.Size(275, 395);
            this.tabProgram.TabIndex = 3;
            this.tabProgram.Text = "Program";
            this.tabProgram.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 454);
            this.Controls.Add(this.spPanMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Scarlet Digital Assistent Configurator";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.spPanMain.Panel1.ResumeLayout(false);
            this.spPanMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spPanMain)).EndInit();
            this.spPanMain.ResumeLayout(false);
            this.tabDictionaryInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer spPanMain;
        private System.Windows.Forms.TreeView treeItems;
        private System.Windows.Forms.TabControl tabDictionaryInfo;
        private System.Windows.Forms.TabPage tabInput;
        private System.Windows.Forms.TabPage tabOutput;
        private System.Windows.Forms.TabPage tabChoice;
        private System.Windows.Forms.TabPage tabProgram;
    }
}

