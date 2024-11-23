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
            this.label1 = new Label();
            this.comboBoxDataBase = new ComboBox();
            this.tabPage2 = new TabPage();
            this.button1 = new Button();
            this.textBoxWebLog = new TextBox();
            this.label2 = new Label();
            this.comboBox1 = new ComboBox();
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
            this.contextMenuStripSysTray.Size = new Size(175, 76);
            // 
            // öffnenToolStripMenuItem
            // 
            this.öffnenToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            this.öffnenToolStripMenuItem.Size = new Size(174, 36);
            this.öffnenToolStripMenuItem.Text = "Öffnen";
            this.öffnenToolStripMenuItem.Click += this.öffnenToolStripMenuItem_Click;
            // 
            // schließenToolStripMenuItem
            // 
            this.schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            this.schließenToolStripMenuItem.Size = new Size(174, 36);
            this.schließenToolStripMenuItem.Text = "Schließen";
            this.schließenToolStripMenuItem.Click += this.schließenToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Margin = new Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(1929, 1108);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBoxDataBase);
            this.tabPage1.Location = new Point(4, 39);
            this.tabPage1.Margin = new Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3, 4, 3, 4);
            this.tabPage1.Size = new Size(1921, 1065);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(9, 22);
            this.label1.Margin = new Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(100, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Database";
            // 
            // comboBoxDataBase
            // 
            this.comboBoxDataBase.FormattingEnabled = true;
            this.comboBoxDataBase.Location = new Point(113, 16);
            this.comboBoxDataBase.Margin = new Padding(5, 6, 5, 6);
            this.comboBoxDataBase.Name = "comboBoxDataBase";
            this.comboBoxDataBase.Size = new Size(205, 38);
            this.comboBoxDataBase.TabIndex = 0;
            this.comboBoxDataBase.SelectedIndexChanged += this.comboBoxDataBase_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.textBoxWebLog);
            this.tabPage2.Location = new Point(4, 39);
            this.tabPage2.Margin = new Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3, 4, 3, 4);
            this.tabPage2.Size = new Size(1921, 1065);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Consol Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new Point(1629, 6);
            this.button1.Margin = new Padding(5, 6, 5, 6);
            this.button1.Name = "button1";
            this.button1.Size = new Size(129, 46);
            this.button1.TabIndex = 1;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += this.button1_Click;
            // 
            // textBoxWebLog
            // 
            this.textBoxWebLog.Location = new Point(7, 6);
            this.textBoxWebLog.Margin = new Padding(3, 4, 3, 4);
            this.textBoxWebLog.Multiline = true;
            this.textBoxWebLog.Name = "textBoxWebLog";
            this.textBoxWebLog.ScrollBars = ScrollBars.Vertical;
            this.textBoxWebLog.Size = new Size(1610, 1034);
            this.textBoxWebLog.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(16, 72);
            this.label2.Margin = new Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(90, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "Drucker:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(113, 66);
            this.comboBox1.Margin = new Padding(5, 6, 5, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(686, 38);
            this.comboBox1.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(12F, 30F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1929, 1108);
            this.Controls.Add(this.tabControl1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(3, 4, 3, 4);
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
    }
}