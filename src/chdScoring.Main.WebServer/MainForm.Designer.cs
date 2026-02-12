namespace chdScoring.Main.WebServer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIconMain = new NotifyIcon(this.components);
            this.contextMenuStripSysTray = new ContextMenuStrip(this.components);
            this.öffnenToolStripMenuItem = new ToolStripMenuItem();
            this.schließenToolStripMenuItem = new ToolStripMenuItem();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.checkBoxAutoPrint = new CheckBox();
            this.label2 = new Label();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.comboBoxDataBase = new ComboBox();
            this.tabPage2 = new TabPage();
            this.button1 = new Button();
            this.textBoxWebLog = new TextBox();
            this.openFileDialogImportCsv = new OpenFileDialog();
            this.buttonImportCsv = new Button();
            this.contextMenuStripSysTray.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.BalloonTipIcon = ToolTipIcon.Info;
            this.notifyIconMain.BalloonTipText = "chdScoring";
            this.notifyIconMain.BalloonTipTitle = "chdScoring Title";
            this.notifyIconMain.ContextMenuStrip = this.contextMenuStripSysTray;
            this.notifyIconMain.Icon = (Icon)resources.GetObject("notifyIconMain.Icon");
            this.notifyIconMain.Text = "chdScoring";
            this.notifyIconMain.MouseDoubleClick += this.notifyIconMain_MouseDoubleClick;
            // 
            // contextMenuStripSysTray
            // 
            this.contextMenuStripSysTray.ImageScalingSize = new Size(28, 28);
            this.contextMenuStripSysTray.Items.AddRange(new ToolStripItem[] { this.öffnenToolStripMenuItem, this.schließenToolStripMenuItem });
            this.contextMenuStripSysTray.Name = "contextMenuStripSysTray";
            this.contextMenuStripSysTray.Size = new Size(126, 48);
            // 
            // öffnenToolStripMenuItem
            // 
            this.öffnenToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            this.öffnenToolStripMenuItem.Size = new Size(125, 22);
            this.öffnenToolStripMenuItem.Text = "Öffnen";
            this.öffnenToolStripMenuItem.Click += this.öffnenToolStripMenuItem_Click;
            // 
            // schließenToolStripMenuItem
            // 
            this.schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            this.schließenToolStripMenuItem.Size = new Size(125, 22);
            this.schließenToolStripMenuItem.Text = "Schließen";
            this.schließenToolStripMenuItem.Click += this.schließenToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Margin = new Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(1125, 554);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonImportCsv);
            this.tabPage1.Controls.Add(this.checkBoxAutoPrint);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBoxDataBase);
            this.tabPage1.Location = new Point(4, 24);
            this.tabPage1.Margin = new Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(2, 2, 2, 2);
            this.tabPage1.Size = new Size(1117, 526);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoPrint
            // 
            this.checkBoxAutoPrint.AutoSize = true;
            this.checkBoxAutoPrint.Location = new Point(481, 36);
            this.checkBoxAutoPrint.Margin = new Padding(2, 2, 2, 2);
            this.checkBoxAutoPrint.Name = "checkBoxAutoPrint";
            this.checkBoxAutoPrint.Size = new Size(80, 19);
            this.checkBoxAutoPrint.TabIndex = 4;
            this.checkBoxAutoPrint.Text = "Auto Print";
            this.checkBoxAutoPrint.UseVisualStyleBackColor = true;
            this.checkBoxAutoPrint.CheckedChanged += this.checkBoxAutoPrint_CheckedChanged;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new Size(51, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Drucker:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(66, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(402, 23);
            this.comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(55, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Database";
            // 
            // comboBoxDataBase
            // 
            this.comboBoxDataBase.FormattingEnabled = true;
            this.comboBoxDataBase.Location = new Point(66, 8);
            this.comboBoxDataBase.Name = "comboBoxDataBase";
            this.comboBoxDataBase.Size = new Size(121, 23);
            this.comboBoxDataBase.TabIndex = 0;
            this.comboBoxDataBase.SelectedIndexChanged += this.comboBoxDataBase_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.textBoxWebLog);
            this.tabPage2.Location = new Point(4, 24);
            this.tabPage2.Margin = new Padding(2, 2, 2, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(2, 2, 2, 2);
            this.tabPage2.Size = new Size(1117, 526);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Consol Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new Point(950, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += this.button1_Click;
            // 
            // textBoxWebLog
            // 
            this.textBoxWebLog.Location = new Point(4, 3);
            this.textBoxWebLog.Margin = new Padding(2, 2, 2, 2);
            this.textBoxWebLog.Multiline = true;
            this.textBoxWebLog.Name = "textBoxWebLog";
            this.textBoxWebLog.ScrollBars = ScrollBars.Vertical;
            this.textBoxWebLog.Size = new Size(941, 519);
            this.textBoxWebLog.TabIndex = 0;
            // 
            // openFileDialogImportCsv
            // 
            this.openFileDialogImportCsv.FileName = "openFileDialogImportCsv";
            // 
            // buttonImportCsv
            // 
            this.buttonImportCsv.Location = new Point(9, 75);
            this.buttonImportCsv.Name = "buttonImportCsv";
            this.buttonImportCsv.Size = new Size(75, 23);
            this.buttonImportCsv.TabIndex = 5;
            this.buttonImportCsv.Text = "CSV Import";
            this.buttonImportCsv.UseVisualStyleBackColor = true;
            this.buttonImportCsv.Click += this.buttonImportCsv_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1125, 554);
            this.Controls.Add(this.tabControl1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "chdScoring";
            this.contextMenuStripSysTray.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private NotifyIcon notifyIconMain;
        private ContextMenuStrip contextMenuStripSysTray;
        private ToolStripMenuItem schließenToolStripMenuItem;
        private ToolStripMenuItem öffnenToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBoxWebLog;
        private Button button1;
        private Label label1;
        private ComboBox comboBoxDataBase;
        private Label label2;
        private ComboBox comboBox1;
        private CheckBox checkBoxAutoPrint;
        private OpenFileDialog openFileDialogImportCsv;
        private Button buttonImportCsv;
    }
}