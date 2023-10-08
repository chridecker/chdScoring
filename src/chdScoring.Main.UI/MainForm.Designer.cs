namespace chdScoring.Main.UI
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
            notifyIconMain = new NotifyIcon(this.components);
            contextMenuStripSysTray = new ContextMenuStrip(this.components);
            öffnenToolStripMenuItem = new ToolStripMenuItem();
            schließenToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            textBoxWebLog = new TextBox();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            contextMenuStripSysTray.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIconMain
            // 
            notifyIconMain.BalloonTipIcon = ToolTipIcon.Info;
            notifyIconMain.BalloonTipText = "chdScoring";
            notifyIconMain.BalloonTipTitle = "chdScoring Title";
            notifyIconMain.ContextMenuStrip = contextMenuStripSysTray;
            notifyIconMain.Icon = (Icon)resources.GetObject("notifyIconMain.Icon");
            notifyIconMain.Text = "Text";
            notifyIconMain.Visible = true;
            notifyIconMain.MouseDoubleClick += this.notifyIconMain_MouseDoubleClick;
            // 
            // contextMenuStripSysTray
            // 
            contextMenuStripSysTray.ImageScalingSize = new Size(28, 28);
            contextMenuStripSysTray.Items.AddRange(new ToolStripItem[] { öffnenToolStripMenuItem, schließenToolStripMenuItem });
            contextMenuStripSysTray.Name = "contextMenuStripSysTray";
            contextMenuStripSysTray.Size = new Size(175, 76);
            // 
            // öffnenToolStripMenuItem
            // 
            öffnenToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            öffnenToolStripMenuItem.Size = new Size(174, 36);
            öffnenToolStripMenuItem.Text = "Öffnen";
            // 
            // schließenToolStripMenuItem
            // 
            schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            schließenToolStripMenuItem.Size = new Size(174, 36);
            schließenToolStripMenuItem.Text = "Schließen";
            schließenToolStripMenuItem.Click += this.schließenToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1929, 1109);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(pictureBox1);
            tabPage1.Location = new Point(4, 39);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1921, 1066);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBoxWebLog);
            tabPage2.Location = new Point(4, 39);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1921, 1066);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxWebLog
            // 
            textBoxWebLog.Location = new Point(6, 6);
            textBoxWebLog.Multiline = true;
            textBoxWebLog.Name = "textBoxWebLog";
            textBoxWebLog.Size = new Size(1446, 869);
            textBoxWebLog.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(881, 341);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(589, 343);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(867, 199);
            button1.Name = "button1";
            button1.Size = new Size(131, 40);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += this.button1_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(12F, 30F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1929, 1109);
            this.Controls.Add(tabControl1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "MainForm";
            this.Text = "chdScoring";
            contextMenuStripSysTray.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
        private PictureBox pictureBox1;
        private Button button1;
    }
}