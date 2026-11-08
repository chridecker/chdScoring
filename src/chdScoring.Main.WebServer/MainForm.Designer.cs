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
            this.webModuleControlServer = new chd.Base.Hosting.Forms.Controls.WebModuleControl();
            this.buttonImportCsv = new Button();
            this.checkBoxAutoPrint = new CheckBox();
            this.label2 = new Label();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.comboBoxDataBase = new ComboBox();
            this.tabPage2 = new TabPage();
            this.channelLogControl1 = new chd.Base.Hosting.Forms.Controls.ChannelLogControl();
            this.tabPage3 = new TabPage();
            this.dataGridViewDevices = new DataGridView();
            this.hostNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.apiKeyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.lastUpdateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.User = new DataGridViewTextBoxColumn();
            this.CurrentSite = new DataGridViewTextBoxColumn();
            this.batteryLevelDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.batteryChargingDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.currentVersionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.currentLocalTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.platformDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.bindingSourceDevices = new BindingSource(this.components);
            this.tabPage4 = new TabPage();
            this.dataGridView1 = new DataGridView();
            this.idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.keyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.roleDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.judgeIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.surnameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.lastnameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.redirectDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.bindingSourceApiKeys = new BindingSource(this.components);
            this.openFileDialogImportCsv = new OpenFileDialog();
            this.contextMenuStripSysTray.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewDevices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.bindingSourceDevices).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.bindingSourceApiKeys).BeginInit();
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
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
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
            this.tabPage1.Controls.Add(this.webModuleControlServer);
            this.tabPage1.Controls.Add(this.buttonImportCsv);
            this.tabPage1.Controls.Add(this.checkBoxAutoPrint);
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
            // webModuleControlServer
            // 
            this.webModuleControlServer.ChoosenIP = "All (0.0.0.0)";
            this.webModuleControlServer.Location = new Point(8, 10);
            this.webModuleControlServer.Margin = new Padding(5, 6, 5, 6);
            this.webModuleControlServer.ModuleDefaultPort = 8081;
            this.webModuleControlServer.ModuleName = "ServerModule";
            this.webModuleControlServer.Name = "webModuleControlServer";
            this.webModuleControlServer.Size = new Size(608, 103);
            this.webModuleControlServer.TabIndex = 6;
            // 
            // buttonImportCsv
            // 
            this.buttonImportCsv.Location = new Point(16, 275);
            this.buttonImportCsv.Margin = new Padding(5, 6, 5, 6);
            this.buttonImportCsv.Name = "buttonImportCsv";
            this.buttonImportCsv.Size = new Size(129, 46);
            this.buttonImportCsv.TabIndex = 5;
            this.buttonImportCsv.Text = "CSV Import";
            this.buttonImportCsv.UseVisualStyleBackColor = true;
            this.buttonImportCsv.Click += this.buttonImportCsv_Click;
            // 
            // checkBoxAutoPrint
            // 
            this.checkBoxAutoPrint.AutoSize = true;
            this.checkBoxAutoPrint.Location = new Point(826, 197);
            this.checkBoxAutoPrint.Margin = new Padding(3, 4, 3, 4);
            this.checkBoxAutoPrint.Name = "checkBoxAutoPrint";
            this.checkBoxAutoPrint.Size = new Size(133, 34);
            this.checkBoxAutoPrint.TabIndex = 4;
            this.checkBoxAutoPrint.Text = "Auto Print";
            this.checkBoxAutoPrint.UseVisualStyleBackColor = true;
            this.checkBoxAutoPrint.CheckedChanged += this.checkBoxAutoPrint_CheckedChanged;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(16, 197);
            this.label2.Margin = new Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(90, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "Drucker:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(114, 191);
            this.comboBox1.Margin = new Padding(5, 6, 5, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(686, 38);
            this.comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(10, 147);
            this.label1.Margin = new Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(100, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Database";
            // 
            // comboBoxDataBase
            // 
            this.comboBoxDataBase.FormattingEnabled = true;
            this.comboBoxDataBase.Location = new Point(114, 141);
            this.comboBoxDataBase.Margin = new Padding(5, 6, 5, 6);
            this.comboBoxDataBase.Name = "comboBoxDataBase";
            this.comboBoxDataBase.Size = new Size(205, 38);
            this.comboBoxDataBase.TabIndex = 0;
            this.comboBoxDataBase.SelectedIndexChanged += this.comboBoxDataBase_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.channelLogControl1);
            this.tabPage2.Location = new Point(4, 39);
            this.tabPage2.Margin = new Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3, 4, 3, 4);
            this.tabPage2.Size = new Size(1921, 1065);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // channelLogControl1
            // 
            this.channelLogControl1.Dock = DockStyle.Fill;
            this.channelLogControl1.Location = new Point(3, 4);
            this.channelLogControl1.Margin = new Padding(5, 6, 5, 6);
            this.channelLogControl1.Name = "channelLogControl1";
            this.channelLogControl1.Size = new Size(1915, 1057);
            this.channelLogControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridViewDevices);
            this.tabPage3.Location = new Point(4, 39);
            this.tabPage3.Margin = new Padding(5, 6, 5, 6);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new Padding(5, 6, 5, 6);
            this.tabPage3.Size = new Size(1921, 1065);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Devices";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridViewDevices
            // 
            this.dataGridViewDevices.AllowUserToAddRows = false;
            this.dataGridViewDevices.AllowUserToDeleteRows = false;
            this.dataGridViewDevices.AutoGenerateColumns = false;
            this.dataGridViewDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDevices.Columns.AddRange(new DataGridViewColumn[] { this.hostNameDataGridViewTextBoxColumn, this.apiKeyDataGridViewTextBoxColumn, this.lastUpdateDataGridViewTextBoxColumn, this.User, this.CurrentSite, this.batteryLevelDataGridViewTextBoxColumn, this.batteryChargingDataGridViewTextBoxColumn, this.currentVersionDataGridViewTextBoxColumn, this.currentLocalTimeDataGridViewTextBoxColumn, this.platformDataGridViewTextBoxColumn });
            this.dataGridViewDevices.DataSource = this.bindingSourceDevices;
            this.dataGridViewDevices.Dock = DockStyle.Fill;
            this.dataGridViewDevices.Location = new Point(5, 6);
            this.dataGridViewDevices.Margin = new Padding(5, 6, 5, 6);
            this.dataGridViewDevices.Name = "dataGridViewDevices";
            this.dataGridViewDevices.ReadOnly = true;
            this.dataGridViewDevices.RowHeadersWidth = 72;
            this.dataGridViewDevices.Size = new Size(1911, 1053);
            this.dataGridViewDevices.TabIndex = 0;
            // 
            // hostNameDataGridViewTextBoxColumn
            // 
            this.hostNameDataGridViewTextBoxColumn.DataPropertyName = "HostName";
            this.hostNameDataGridViewTextBoxColumn.HeaderText = "HostName";
            this.hostNameDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.hostNameDataGridViewTextBoxColumn.Name = "hostNameDataGridViewTextBoxColumn";
            this.hostNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.hostNameDataGridViewTextBoxColumn.Width = 175;
            // 
            // apiKeyDataGridViewTextBoxColumn
            // 
            this.apiKeyDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.apiKeyDataGridViewTextBoxColumn.DataPropertyName = "ApiKey";
            this.apiKeyDataGridViewTextBoxColumn.HeaderText = "X-ApiKey";
            this.apiKeyDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.apiKeyDataGridViewTextBoxColumn.Name = "apiKeyDataGridViewTextBoxColumn";
            this.apiKeyDataGridViewTextBoxColumn.ReadOnly = true;
            this.apiKeyDataGridViewTextBoxColumn.Width = 138;
            // 
            // lastUpdateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.lastUpdateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdate";
            this.lastUpdateDataGridViewTextBoxColumn.HeaderText = "LastUpdate";
            this.lastUpdateDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.lastUpdateDataGridViewTextBoxColumn.Name = "lastUpdateDataGridViewTextBoxColumn";
            this.lastUpdateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdateDataGridViewTextBoxColumn.Width = 158;
            // 
            // User
            // 
            this.User.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.User.DataPropertyName = "User";
            this.User.HeaderText = "User";
            this.User.MinimumWidth = 9;
            this.User.Name = "User";
            this.User.ReadOnly = true;
            this.User.Width = 9;
            // 
            // CurrentSite
            // 
            this.CurrentSite.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.CurrentSite.DataPropertyName = "CurrentSite";
            this.CurrentSite.HeaderText = "CurrentSite";
            this.CurrentSite.MinimumWidth = 9;
            this.CurrentSite.Name = "CurrentSite";
            this.CurrentSite.ReadOnly = true;
            this.CurrentSite.Width = 9;
            // 
            // batteryLevelDataGridViewTextBoxColumn
            // 
            this.batteryLevelDataGridViewTextBoxColumn.DataPropertyName = "BatteryLevel";
            this.batteryLevelDataGridViewTextBoxColumn.HeaderText = "BatteryLevel";
            this.batteryLevelDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.batteryLevelDataGridViewTextBoxColumn.Name = "batteryLevelDataGridViewTextBoxColumn";
            this.batteryLevelDataGridViewTextBoxColumn.ReadOnly = true;
            this.batteryLevelDataGridViewTextBoxColumn.Width = 175;
            // 
            // batteryChargingDataGridViewTextBoxColumn
            // 
            this.batteryChargingDataGridViewTextBoxColumn.DataPropertyName = "BatteryCharging";
            this.batteryChargingDataGridViewTextBoxColumn.HeaderText = "BatteryCharging";
            this.batteryChargingDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.batteryChargingDataGridViewTextBoxColumn.Name = "batteryChargingDataGridViewTextBoxColumn";
            this.batteryChargingDataGridViewTextBoxColumn.ReadOnly = true;
            this.batteryChargingDataGridViewTextBoxColumn.Width = 175;
            // 
            // currentVersionDataGridViewTextBoxColumn
            // 
            this.currentVersionDataGridViewTextBoxColumn.DataPropertyName = "CurrentVersion";
            this.currentVersionDataGridViewTextBoxColumn.HeaderText = "CurrentVersion";
            this.currentVersionDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.currentVersionDataGridViewTextBoxColumn.Name = "currentVersionDataGridViewTextBoxColumn";
            this.currentVersionDataGridViewTextBoxColumn.ReadOnly = true;
            this.currentVersionDataGridViewTextBoxColumn.Width = 175;
            // 
            // currentLocalTimeDataGridViewTextBoxColumn
            // 
            this.currentLocalTimeDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.currentLocalTimeDataGridViewTextBoxColumn.DataPropertyName = "CurrentLocalTime";
            this.currentLocalTimeDataGridViewTextBoxColumn.HeaderText = "CurrentLocalTime";
            this.currentLocalTimeDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.currentLocalTimeDataGridViewTextBoxColumn.Name = "currentLocalTimeDataGridViewTextBoxColumn";
            this.currentLocalTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.currentLocalTimeDataGridViewTextBoxColumn.Width = 216;
            // 
            // platformDataGridViewTextBoxColumn
            // 
            this.platformDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.platformDataGridViewTextBoxColumn.DataPropertyName = "Platform";
            this.platformDataGridViewTextBoxColumn.HeaderText = "Platform";
            this.platformDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.platformDataGridViewTextBoxColumn.Name = "platformDataGridViewTextBoxColumn";
            this.platformDataGridViewTextBoxColumn.ReadOnly = true;
            this.platformDataGridViewTextBoxColumn.Width = 133;
            // 
            // bindingSourceDevices
            // 
            this.bindingSourceDevices.DataSource = typeof(Contracts.Dtos.DeviceStatusDto);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView1);
            this.tabPage4.Location = new Point(4, 39);
            this.tabPage4.Margin = new Padding(5, 6, 5, 6);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new Padding(5, 6, 5, 6);
            this.tabPage4.Size = new Size(1921, 1065);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Api-Keys";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { this.idDataGridViewTextBoxColumn, this.keyDataGridViewTextBoxColumn, this.roleDataGridViewTextBoxColumn, this.judgeIdDataGridViewTextBoxColumn, this.surnameDataGridViewTextBoxColumn, this.lastnameDataGridViewTextBoxColumn, this.redirectDataGridViewTextBoxColumn });
            this.dataGridView1.DataSource = this.bindingSourceApiKeys;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(5, 6);
            this.dataGridView1.Margin = new Padding(5, 6, 5, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 72;
            this.dataGridView1.Size = new Size(1911, 1053);
            this.dataGridView1.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Width = 175;
            // 
            // keyDataGridViewTextBoxColumn
            // 
            this.keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            this.keyDataGridViewTextBoxColumn.HeaderText = "Key";
            this.keyDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            this.keyDataGridViewTextBoxColumn.Width = 175;
            // 
            // roleDataGridViewTextBoxColumn
            // 
            this.roleDataGridViewTextBoxColumn.DataPropertyName = "Role";
            this.roleDataGridViewTextBoxColumn.HeaderText = "Role";
            this.roleDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.roleDataGridViewTextBoxColumn.Name = "roleDataGridViewTextBoxColumn";
            this.roleDataGridViewTextBoxColumn.Width = 175;
            // 
            // judgeIdDataGridViewTextBoxColumn
            // 
            this.judgeIdDataGridViewTextBoxColumn.DataPropertyName = "JudgeId";
            this.judgeIdDataGridViewTextBoxColumn.HeaderText = "JudgeId";
            this.judgeIdDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.judgeIdDataGridViewTextBoxColumn.Name = "judgeIdDataGridViewTextBoxColumn";
            this.judgeIdDataGridViewTextBoxColumn.Width = 175;
            // 
            // surnameDataGridViewTextBoxColumn
            // 
            this.surnameDataGridViewTextBoxColumn.DataPropertyName = "Surname";
            this.surnameDataGridViewTextBoxColumn.HeaderText = "Surname";
            this.surnameDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.surnameDataGridViewTextBoxColumn.Name = "surnameDataGridViewTextBoxColumn";
            this.surnameDataGridViewTextBoxColumn.Width = 175;
            // 
            // lastnameDataGridViewTextBoxColumn
            // 
            this.lastnameDataGridViewTextBoxColumn.DataPropertyName = "Lastname";
            this.lastnameDataGridViewTextBoxColumn.HeaderText = "Lastname";
            this.lastnameDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.lastnameDataGridViewTextBoxColumn.Name = "lastnameDataGridViewTextBoxColumn";
            this.lastnameDataGridViewTextBoxColumn.Width = 175;
            // 
            // redirectDataGridViewTextBoxColumn
            // 
            this.redirectDataGridViewTextBoxColumn.DataPropertyName = "Redirect";
            this.redirectDataGridViewTextBoxColumn.HeaderText = "Redirect";
            this.redirectDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.redirectDataGridViewTextBoxColumn.Name = "redirectDataGridViewTextBoxColumn";
            this.redirectDataGridViewTextBoxColumn.Width = 175;
            // 
            // bindingSourceApiKeys
            // 
            this.bindingSourceApiKeys.DataSource = typeof(Contracts.Dtos.ApiKeyDto);
            // 
            // openFileDialogImportCsv
            // 
            this.openFileDialogImportCsv.FileName = "openFileDialogImportCsv";
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
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewDevices).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.bindingSourceDevices).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.bindingSourceApiKeys).EndInit();
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
        private chd.Base.Hosting.Forms.Controls.WebModuleControl webModuleControlServer;
        private chd.Base.Hosting.Forms.Controls.ChannelLogControl channelLogControl1;
    }
}