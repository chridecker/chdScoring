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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            notifyIconMain = new NotifyIcon(components);
            contextMenuStripSysTray = new ContextMenuStrip(components);
            öffnenToolStripMenuItem = new ToolStripMenuItem();
            schließenToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            buttonImportCsv = new Button();
            checkBoxAutoPrint = new CheckBox();
            label2 = new Label();
            comboBox1 = new ComboBox();
            label1 = new Label();
            comboBoxDataBase = new ComboBox();
            tabPage2 = new TabPage();
            button1 = new Button();
            textBoxWebLog = new TextBox();
            tabPage3 = new TabPage();
            dataGridViewDevices = new DataGridView();
            hostNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            apiKeyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            platformDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lastUpdateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            User = new DataGridViewTextBoxColumn();
            CurrentSite = new DataGridViewTextBoxColumn();
            batteryLevelDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            batteryChargingDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            currentVersionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            currentLocalTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            bindingSourceDevices = new BindingSource(components);
            tabPage4 = new TabPage();
            dataGridView1 = new DataGridView();
            openFileDialogImportCsv = new OpenFileDialog();
            bindingSourceApiKeys = new BindingSource(components);
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            keyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            roleDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            judgeIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            surnameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lastnameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            redirectDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            contextMenuStripSysTray.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDevices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceDevices).BeginInit();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceApiKeys).BeginInit();
            SuspendLayout();
            // 
            // notifyIconMain
            // 
            notifyIconMain.BalloonTipIcon = ToolTipIcon.Info;
            notifyIconMain.BalloonTipText = "chdScoring";
            notifyIconMain.BalloonTipTitle = "chdScoring Title";
            notifyIconMain.ContextMenuStrip = contextMenuStripSysTray;
            notifyIconMain.Icon = (Icon)resources.GetObject("notifyIconMain.Icon");
            notifyIconMain.Text = "chdScoring";
            notifyIconMain.MouseDoubleClick += notifyIconMain_MouseDoubleClick;
            // 
            // contextMenuStripSysTray
            // 
            contextMenuStripSysTray.ImageScalingSize = new Size(28, 28);
            contextMenuStripSysTray.Items.AddRange(new ToolStripItem[] { öffnenToolStripMenuItem, schließenToolStripMenuItem });
            contextMenuStripSysTray.Name = "contextMenuStripSysTray";
            contextMenuStripSysTray.Size = new Size(126, 48);
            // 
            // öffnenToolStripMenuItem
            // 
            öffnenToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            öffnenToolStripMenuItem.Size = new Size(125, 22);
            öffnenToolStripMenuItem.Text = "Öffnen";
            öffnenToolStripMenuItem.Click += öffnenToolStripMenuItem_Click;
            // 
            // schließenToolStripMenuItem
            // 
            schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            schließenToolStripMenuItem.Size = new Size(125, 22);
            schließenToolStripMenuItem.Text = "Schließen";
            schließenToolStripMenuItem.Click += schließenToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1125, 554);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(buttonImportCsv);
            tabPage1.Controls.Add(checkBoxAutoPrint);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(comboBox1);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(comboBoxDataBase);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2);
            tabPage1.Size = new Size(1117, 526);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Settings";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonImportCsv
            // 
            buttonImportCsv.Location = new Point(9, 75);
            buttonImportCsv.Name = "buttonImportCsv";
            buttonImportCsv.Size = new Size(75, 23);
            buttonImportCsv.TabIndex = 5;
            buttonImportCsv.Text = "CSV Import";
            buttonImportCsv.UseVisualStyleBackColor = true;
            buttonImportCsv.Click += buttonImportCsv_Click;
            // 
            // checkBoxAutoPrint
            // 
            checkBoxAutoPrint.AutoSize = true;
            checkBoxAutoPrint.Location = new Point(481, 36);
            checkBoxAutoPrint.Margin = new Padding(2);
            checkBoxAutoPrint.Name = "checkBoxAutoPrint";
            checkBoxAutoPrint.Size = new Size(80, 19);
            checkBoxAutoPrint.TabIndex = 4;
            checkBoxAutoPrint.Text = "Auto Print";
            checkBoxAutoPrint.UseVisualStyleBackColor = true;
            checkBoxAutoPrint.CheckedChanged += checkBoxAutoPrint_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 36);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 3;
            label2.Text = "Drucker:";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(66, 33);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(402, 23);
            comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(5, 11);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 1;
            label1.Text = "Database";
            // 
            // comboBoxDataBase
            // 
            comboBoxDataBase.FormattingEnabled = true;
            comboBoxDataBase.Location = new Point(66, 8);
            comboBoxDataBase.Name = "comboBoxDataBase";
            comboBoxDataBase.Size = new Size(121, 23);
            comboBoxDataBase.TabIndex = 0;
            comboBoxDataBase.SelectedIndexChanged += comboBoxDataBase_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(textBoxWebLog);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(2);
            tabPage2.Size = new Size(1117, 526);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Consol Log";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(950, 3);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Clear";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBoxWebLog
            // 
            textBoxWebLog.Location = new Point(4, 3);
            textBoxWebLog.Margin = new Padding(2);
            textBoxWebLog.Multiline = true;
            textBoxWebLog.Name = "textBoxWebLog";
            textBoxWebLog.ScrollBars = ScrollBars.Vertical;
            textBoxWebLog.Size = new Size(941, 519);
            textBoxWebLog.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dataGridViewDevices);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1117, 526);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Devices";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridViewDevices
            // 
            dataGridViewDevices.AllowUserToAddRows = false;
            dataGridViewDevices.AllowUserToDeleteRows = false;
            dataGridViewDevices.AutoGenerateColumns = false;
            dataGridViewDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDevices.Columns.AddRange(new DataGridViewColumn[] { hostNameDataGridViewTextBoxColumn, apiKeyDataGridViewTextBoxColumn, lastUpdateDataGridViewTextBoxColumn, User, CurrentSite, batteryLevelDataGridViewTextBoxColumn, batteryChargingDataGridViewTextBoxColumn, currentVersionDataGridViewTextBoxColumn, currentLocalTimeDataGridViewTextBoxColumn, platformDataGridViewTextBoxColumn });
            dataGridViewDevices.DataSource = bindingSourceDevices;
            dataGridViewDevices.Dock = DockStyle.Fill;
            dataGridViewDevices.Location = new Point(3, 3);
            dataGridViewDevices.Name = "dataGridViewDevices";
            dataGridViewDevices.ReadOnly = true;
            dataGridViewDevices.Size = new Size(1111, 520);
            dataGridViewDevices.TabIndex = 0;
            // 
            // hostNameDataGridViewTextBoxColumn
            // 
            hostNameDataGridViewTextBoxColumn.DataPropertyName = "HostName";
            hostNameDataGridViewTextBoxColumn.HeaderText = "HostName";
            hostNameDataGridViewTextBoxColumn.Name = "hostNameDataGridViewTextBoxColumn";
            hostNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // apiKeyDataGridViewTextBoxColumn
            // 
            apiKeyDataGridViewTextBoxColumn.DataPropertyName = "ApiKey";
            apiKeyDataGridViewTextBoxColumn.HeaderText = "X-ApiKey";
            apiKeyDataGridViewTextBoxColumn.Name = "apiKeyDataGridViewTextBoxColumn";
            apiKeyDataGridViewTextBoxColumn.ReadOnly = true;
            apiKeyDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // 
            // platformDataGridViewTextBoxColumn
            // 
            platformDataGridViewTextBoxColumn.DataPropertyName = "Platform";
            platformDataGridViewTextBoxColumn.HeaderText = "Platform";
            platformDataGridViewTextBoxColumn.Name = "platformDataGridViewTextBoxColumn";
            platformDataGridViewTextBoxColumn.ReadOnly = true;
            platformDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // 
            // lastUpdateDataGridViewTextBoxColumn
            // 
            lastUpdateDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            lastUpdateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdate";
            lastUpdateDataGridViewTextBoxColumn.HeaderText = "LastUpdate";
            lastUpdateDataGridViewTextBoxColumn.Name = "lastUpdateDataGridViewTextBoxColumn";
            lastUpdateDataGridViewTextBoxColumn.ReadOnly = true;
            lastUpdateDataGridViewTextBoxColumn.Width = 5;
            // 
            // User
            // 
            User.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            User.DataPropertyName = "User";
            User.HeaderText = "User";
            User.Name = "User";
            User.ReadOnly = true;
            User.Width = 5;
            // 
            // CurrentSite
            // 
            CurrentSite.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            CurrentSite.DataPropertyName = "CurrentSite";
            CurrentSite.HeaderText = "CurrentSite";
            CurrentSite.Name = "CurrentSite";
            CurrentSite.ReadOnly = true;
            CurrentSite.Width = 5;
            // 
            // batteryLevelDataGridViewTextBoxColumn
            // 
            batteryLevelDataGridViewTextBoxColumn.DataPropertyName = "BatteryLevel";
            batteryLevelDataGridViewTextBoxColumn.HeaderText = "BatteryLevel";
            batteryLevelDataGridViewTextBoxColumn.Name = "batteryLevelDataGridViewTextBoxColumn";
            batteryLevelDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // batteryChargingDataGridViewTextBoxColumn
            // 
            batteryChargingDataGridViewTextBoxColumn.DataPropertyName = "BatteryCharging";
            batteryChargingDataGridViewTextBoxColumn.HeaderText = "BatteryCharging";
            batteryChargingDataGridViewTextBoxColumn.Name = "batteryChargingDataGridViewTextBoxColumn";
            batteryChargingDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currentVersionDataGridViewTextBoxColumn
            // 
            currentVersionDataGridViewTextBoxColumn.DataPropertyName = "CurrentVersion";
            currentVersionDataGridViewTextBoxColumn.HeaderText = "CurrentVersion";
            currentVersionDataGridViewTextBoxColumn.Name = "currentVersionDataGridViewTextBoxColumn";
            currentVersionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currentLocalTimeDataGridViewTextBoxColumn
            // 
            currentLocalTimeDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            currentLocalTimeDataGridViewTextBoxColumn.DataPropertyName = "CurrentLocalTime";
            currentLocalTimeDataGridViewTextBoxColumn.HeaderText = "CurrentLocalTime";
            currentLocalTimeDataGridViewTextBoxColumn.Name = "currentLocalTimeDataGridViewTextBoxColumn";
            currentLocalTimeDataGridViewTextBoxColumn.ReadOnly = true;
            currentLocalTimeDataGridViewTextBoxColumn.Width = 5;
            // 
            // bindingSourceDevices
            // 
            bindingSourceDevices.DataSource = typeof(Contracts.Dtos.DeviceStatusDto);
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dataGridView1);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1117, 526);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Api-Keys";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { idDataGridViewTextBoxColumn, keyDataGridViewTextBoxColumn, roleDataGridViewTextBoxColumn, judgeIdDataGridViewTextBoxColumn, surnameDataGridViewTextBoxColumn, lastnameDataGridViewTextBoxColumn, redirectDataGridViewTextBoxColumn });
            dataGridView1.DataSource = bindingSourceApiKeys;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1111, 520);
            dataGridView1.TabIndex = 0;
            // 
            // openFileDialogImportCsv
            // 
            openFileDialogImportCsv.FileName = "openFileDialogImportCsv";
            // 
            // bindingSourceApiKeys
            // 
            bindingSourceApiKeys.DataSource = typeof(Contracts.Dtos.ApiKeyDto);
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // keyDataGridViewTextBoxColumn
            // 
            keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            keyDataGridViewTextBoxColumn.HeaderText = "Key";
            keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            // 
            // roleDataGridViewTextBoxColumn
            // 
            roleDataGridViewTextBoxColumn.DataPropertyName = "Role";
            roleDataGridViewTextBoxColumn.HeaderText = "Role";
            roleDataGridViewTextBoxColumn.Name = "roleDataGridViewTextBoxColumn";
            // 
            // judgeIdDataGridViewTextBoxColumn
            // 
            judgeIdDataGridViewTextBoxColumn.DataPropertyName = "JudgeId";
            judgeIdDataGridViewTextBoxColumn.HeaderText = "JudgeId";
            judgeIdDataGridViewTextBoxColumn.Name = "judgeIdDataGridViewTextBoxColumn";
            // 
            // surnameDataGridViewTextBoxColumn
            // 
            surnameDataGridViewTextBoxColumn.DataPropertyName = "Surname";
            surnameDataGridViewTextBoxColumn.HeaderText = "Surname";
            surnameDataGridViewTextBoxColumn.Name = "surnameDataGridViewTextBoxColumn";
            // 
            // lastnameDataGridViewTextBoxColumn
            // 
            lastnameDataGridViewTextBoxColumn.DataPropertyName = "Lastname";
            lastnameDataGridViewTextBoxColumn.HeaderText = "Lastname";
            lastnameDataGridViewTextBoxColumn.Name = "lastnameDataGridViewTextBoxColumn";
            // 
            // redirectDataGridViewTextBoxColumn
            // 
            redirectDataGridViewTextBoxColumn.DataPropertyName = "Redirect";
            redirectDataGridViewTextBoxColumn.HeaderText = "Redirect";
            redirectDataGridViewTextBoxColumn.Name = "redirectDataGridViewTextBoxColumn";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 554);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "chdScoring";
            contextMenuStripSysTray.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewDevices).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceDevices).EndInit();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceApiKeys).EndInit();
            ResumeLayout(false);
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
        private TabPage tabPage3;
        private DataGridView dataGridViewDevices;
        private BindingSource bindingSourceDevices;
        private DataGridViewTextBoxColumn hostNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn apiKeyDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn platformDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastUpdateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn User;
        private DataGridViewTextBoxColumn CurrentSite;
        private DataGridViewTextBoxColumn batteryLevelDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn batteryChargingDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn currentVersionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn currentLocalTimeDataGridViewTextBoxColumn;
        private TabPage tabPage4;
        private DataGridView dataGridView1;
        private BindingSource bindingSourceApiKeys;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn roleDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn judgeIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn surnameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastnameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn redirectDataGridViewTextBoxColumn;
    }
}