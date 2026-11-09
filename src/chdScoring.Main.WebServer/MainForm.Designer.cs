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
            webModuleControlWeb = new chd.Base.Hosting.Forms.Controls.WebModuleControl();
            groupBoxAppSettings = new GroupBox();
            textBoxLocalApiUrl = new TextBox();
            label4 = new Label();
            textBoxRounPrintUrl = new TextBox();
            label6 = new Label();
            textBoxDefaultApiKey = new TextBox();
            label3 = new Label();
            webModuleControlServer = new chd.Base.Hosting.Forms.Controls.WebModuleControl();
            buttonImportCsv = new Button();
            checkBoxAutoPrint = new CheckBox();
            label2 = new Label();
            comboBox1 = new ComboBox();
            label1 = new Label();
            comboBoxDataBase = new ComboBox();
            tabPage2 = new TabPage();
            channelLogControl1 = new chd.Base.Hosting.Forms.Controls.ChannelLogControl();
            tabPage3 = new TabPage();
            dataGridViewDevices = new DataGridView();
            hostNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            apiKeyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lastUpdateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            User = new DataGridViewTextBoxColumn();
            CurrentSite = new DataGridViewTextBoxColumn();
            batteryLevelDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            batteryChargingDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            currentVersionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            currentLocalTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            platformDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            bindingSourceDevices = new BindingSource(components);
            bindingSourceApiKeys = new BindingSource(components);
            openFileDialogImportCsv = new OpenFileDialog();
            contextMenuStripSysTray.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBoxAppSettings.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDevices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceDevices).BeginInit();
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
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(2, 2, 2, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1125, 530);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(webModuleControlWeb);
            tabPage1.Controls.Add(groupBoxAppSettings);
            tabPage1.Controls.Add(webModuleControlServer);
            tabPage1.Controls.Add(buttonImportCsv);
            tabPage1.Controls.Add(checkBoxAutoPrint);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(comboBox1);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(comboBoxDataBase);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(2, 2, 2, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2, 2, 2, 2);
            tabPage1.Size = new Size(1117, 502);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Settings";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // webModuleControlWeb
            // 
            webModuleControlWeb.ChoosenIP = "All (0.0.0.0)";
            webModuleControlWeb.Location = new Point(365, 5);
            webModuleControlWeb.ModuleDefaultPort = 8082;
            webModuleControlWeb.ModuleDefaultSslPort = null;
            webModuleControlWeb.ModuleName = "WebModule";
            webModuleControlWeb.Name = "webModuleControlWeb";
            webModuleControlWeb.Size = new Size(351, 52);
            webModuleControlWeb.TabIndex = 8;
            // 
            // groupBoxAppSettings
            // 
            groupBoxAppSettings.Controls.Add(textBoxLocalApiUrl);
            groupBoxAppSettings.Controls.Add(label4);
            groupBoxAppSettings.Controls.Add(textBoxRounPrintUrl);
            groupBoxAppSettings.Controls.Add(label6);
            groupBoxAppSettings.Controls.Add(textBoxDefaultApiKey);
            groupBoxAppSettings.Controls.Add(label3);
            groupBoxAppSettings.Location = new Point(5, 61);
            groupBoxAppSettings.Margin = new Padding(2, 2, 2, 2);
            groupBoxAppSettings.Name = "groupBoxAppSettings";
            groupBoxAppSettings.Padding = new Padding(2, 2, 2, 2);
            groupBoxAppSettings.Size = new Size(711, 64);
            groupBoxAppSettings.TabIndex = 7;
            groupBoxAppSettings.TabStop = false;
            groupBoxAppSettings.Text = "chdScoring";
            // 
            // textBoxLocalApiUrl
            // 
            textBoxLocalApiUrl.Location = new Point(393, 20);
            textBoxLocalApiUrl.Margin = new Padding(2, 2, 2, 2);
            textBoxLocalApiUrl.Name = "textBoxLocalApiUrl";
            textBoxLocalApiUrl.Size = new Size(191, 23);
            textBoxLocalApiUrl.TabIndex = 9;
            textBoxLocalApiUrl.Text = "http://localhost:8081/";
            textBoxLocalApiUrl.TextChanged += textBoxLocalApiUrl_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(301, 20);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(74, 15);
            label4.TabIndex = 8;
            label4.Text = "Local Api Url";
            // 
            // textBoxRounPrintUrl
            // 
            textBoxRounPrintUrl.Location = new Point(95, 40);
            textBoxRounPrintUrl.Margin = new Padding(2, 2, 2, 2);
            textBoxRounPrintUrl.Name = "textBoxRounPrintUrl";
            textBoxRounPrintUrl.Size = new Size(191, 23);
            textBoxRounPrintUrl.TabIndex = 7;
            textBoxRounPrintUrl.TextChanged += textBoxRounPrintUrl_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(4, 40);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(88, 15);
            label6.TabIndex = 6;
            label6.Text = "Round Print Url";
            // 
            // textBoxDefaultApiKey
            // 
            textBoxDefaultApiKey.Location = new Point(95, 20);
            textBoxDefaultApiKey.Margin = new Padding(2, 2, 2, 2);
            textBoxDefaultApiKey.Name = "textBoxDefaultApiKey";
            textBoxDefaultApiKey.Size = new Size(191, 23);
            textBoxDefaultApiKey.TabIndex = 1;
            textBoxDefaultApiKey.TextChanged += textBoxDefaultApiKey_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(4, 20);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(85, 15);
            label3.TabIndex = 0;
            label3.Text = "Default ApiKey";
            // 
            // webModuleControlServer
            // 
            webModuleControlServer.ChoosenIP = "All (0.0.0.0)";
            webModuleControlServer.Location = new Point(5, 5);
            webModuleControlServer.ModuleDefaultPort = 8081;
            webModuleControlServer.ModuleDefaultSslPort = null;
            webModuleControlServer.ModuleName = "ServerModule";
            webModuleControlServer.Name = "webModuleControlServer";
            webModuleControlServer.Size = new Size(355, 52);
            webModuleControlServer.TabIndex = 6;
            // 
            // buttonImportCsv
            // 
            buttonImportCsv.Location = new Point(27, 207);
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
            checkBoxAutoPrint.Location = new Point(500, 168);
            checkBoxAutoPrint.Margin = new Padding(2, 2, 2, 2);
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
            label2.Location = new Point(27, 168);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 3;
            label2.Text = "Drucker:";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(85, 165);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(402, 23);
            comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 143);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 1;
            label1.Text = "Database";
            // 
            // comboBoxDataBase
            // 
            comboBoxDataBase.FormattingEnabled = true;
            comboBoxDataBase.Location = new Point(85, 140);
            comboBoxDataBase.Name = "comboBoxDataBase";
            comboBoxDataBase.Size = new Size(121, 23);
            comboBoxDataBase.TabIndex = 0;
            comboBoxDataBase.SelectedIndexChanged += comboBoxDataBase_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(channelLogControl1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(2, 2, 2, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(2, 2, 2, 2);
            tabPage2.Size = new Size(1117, 526);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Log";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // channelLogControl1
            // 
            channelLogControl1.Dock = DockStyle.Fill;
            channelLogControl1.Location = new Point(2, 2);
            channelLogControl1.Name = "channelLogControl1";
            channelLogControl1.Size = new Size(1113, 522);
            channelLogControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dataGridViewDevices);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3, 3, 3, 3);
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
            dataGridViewDevices.RowHeadersWidth = 72;
            dataGridViewDevices.Size = new Size(1111, 520);
            dataGridViewDevices.TabIndex = 0;
            // 
            // hostNameDataGridViewTextBoxColumn
            // 
            hostNameDataGridViewTextBoxColumn.DataPropertyName = "HostName";
            hostNameDataGridViewTextBoxColumn.HeaderText = "HostName";
            hostNameDataGridViewTextBoxColumn.MinimumWidth = 9;
            hostNameDataGridViewTextBoxColumn.Name = "hostNameDataGridViewTextBoxColumn";
            hostNameDataGridViewTextBoxColumn.ReadOnly = true;
            hostNameDataGridViewTextBoxColumn.Width = 175;
            // 
            // apiKeyDataGridViewTextBoxColumn
            // 
            apiKeyDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            apiKeyDataGridViewTextBoxColumn.DataPropertyName = "ApiKey";
            apiKeyDataGridViewTextBoxColumn.HeaderText = "X-ApiKey";
            apiKeyDataGridViewTextBoxColumn.MinimumWidth = 9;
            apiKeyDataGridViewTextBoxColumn.Name = "apiKeyDataGridViewTextBoxColumn";
            apiKeyDataGridViewTextBoxColumn.ReadOnly = true;
            apiKeyDataGridViewTextBoxColumn.Width = 81;
            // 
            // lastUpdateDataGridViewTextBoxColumn
            // 
            lastUpdateDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            lastUpdateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdate";
            lastUpdateDataGridViewTextBoxColumn.HeaderText = "LastUpdate";
            lastUpdateDataGridViewTextBoxColumn.MinimumWidth = 9;
            lastUpdateDataGridViewTextBoxColumn.Name = "lastUpdateDataGridViewTextBoxColumn";
            lastUpdateDataGridViewTextBoxColumn.ReadOnly = true;
            lastUpdateDataGridViewTextBoxColumn.Width = 91;
            // 
            // User
            // 
            User.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            User.DataPropertyName = "User";
            User.HeaderText = "User";
            User.MinimumWidth = 9;
            User.Name = "User";
            User.ReadOnly = true;
            User.Width = 9;
            // 
            // CurrentSite
            // 
            CurrentSite.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            CurrentSite.DataPropertyName = "CurrentSite";
            CurrentSite.HeaderText = "CurrentSite";
            CurrentSite.MinimumWidth = 9;
            CurrentSite.Name = "CurrentSite";
            CurrentSite.ReadOnly = true;
            CurrentSite.Width = 9;
            // 
            // batteryLevelDataGridViewTextBoxColumn
            // 
            batteryLevelDataGridViewTextBoxColumn.DataPropertyName = "BatteryLevel";
            batteryLevelDataGridViewTextBoxColumn.HeaderText = "BatteryLevel";
            batteryLevelDataGridViewTextBoxColumn.MinimumWidth = 9;
            batteryLevelDataGridViewTextBoxColumn.Name = "batteryLevelDataGridViewTextBoxColumn";
            batteryLevelDataGridViewTextBoxColumn.ReadOnly = true;
            batteryLevelDataGridViewTextBoxColumn.Width = 175;
            // 
            // batteryChargingDataGridViewTextBoxColumn
            // 
            batteryChargingDataGridViewTextBoxColumn.DataPropertyName = "BatteryCharging";
            batteryChargingDataGridViewTextBoxColumn.HeaderText = "BatteryCharging";
            batteryChargingDataGridViewTextBoxColumn.MinimumWidth = 9;
            batteryChargingDataGridViewTextBoxColumn.Name = "batteryChargingDataGridViewTextBoxColumn";
            batteryChargingDataGridViewTextBoxColumn.ReadOnly = true;
            batteryChargingDataGridViewTextBoxColumn.Width = 175;
            // 
            // currentVersionDataGridViewTextBoxColumn
            // 
            currentVersionDataGridViewTextBoxColumn.DataPropertyName = "CurrentVersion";
            currentVersionDataGridViewTextBoxColumn.HeaderText = "CurrentVersion";
            currentVersionDataGridViewTextBoxColumn.MinimumWidth = 9;
            currentVersionDataGridViewTextBoxColumn.Name = "currentVersionDataGridViewTextBoxColumn";
            currentVersionDataGridViewTextBoxColumn.ReadOnly = true;
            currentVersionDataGridViewTextBoxColumn.Width = 175;
            // 
            // currentLocalTimeDataGridViewTextBoxColumn
            // 
            currentLocalTimeDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            currentLocalTimeDataGridViewTextBoxColumn.DataPropertyName = "CurrentLocalTime";
            currentLocalTimeDataGridViewTextBoxColumn.HeaderText = "CurrentLocalTime";
            currentLocalTimeDataGridViewTextBoxColumn.MinimumWidth = 9;
            currentLocalTimeDataGridViewTextBoxColumn.Name = "currentLocalTimeDataGridViewTextBoxColumn";
            currentLocalTimeDataGridViewTextBoxColumn.ReadOnly = true;
            currentLocalTimeDataGridViewTextBoxColumn.Width = 127;
            // 
            // platformDataGridViewTextBoxColumn
            // 
            platformDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            platformDataGridViewTextBoxColumn.DataPropertyName = "Platform";
            platformDataGridViewTextBoxColumn.HeaderText = "Platform";
            platformDataGridViewTextBoxColumn.MinimumWidth = 9;
            platformDataGridViewTextBoxColumn.Name = "platformDataGridViewTextBoxColumn";
            platformDataGridViewTextBoxColumn.ReadOnly = true;
            platformDataGridViewTextBoxColumn.Width = 78;
            // 
            // bindingSourceDevices
            // 
            bindingSourceDevices.DataSource = typeof(Contracts.Dtos.DeviceStatusDto);
            // 
            // bindingSourceApiKeys
            // 
            bindingSourceApiKeys.DataSource = typeof(Contracts.Dtos.ApiKeyDto);
            // 
            // openFileDialogImportCsv
            // 
            openFileDialogImportCsv.FileName = "openFileDialogImportCsv";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 530);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 2, 2, 2);
            Name = "MainForm";
            Text = "chdScoring";
            contextMenuStripSysTray.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBoxAppSettings.ResumeLayout(false);
            groupBoxAppSettings.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewDevices).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceDevices).EndInit();
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
        private BindingSource bindingSourceApiKeys;
        private chd.Base.Hosting.Forms.Controls.WebModuleControl webModuleControlServer;
        private chd.Base.Hosting.Forms.Controls.ChannelLogControl channelLogControl1;
        private GroupBox groupBoxAppSettings;
        private TextBox textBoxDefaultApiKey;
        private Label label3;
        private TextBox textBoxRounPrintUrl;
        private Label label6;
        private chd.Base.Hosting.Forms.Controls.WebModuleControl webModuleControlWeb;
        private TextBox textBoxLocalApiUrl;
        private Label label4;
    }
}