using chdScoring.BusinessLogic.Hubs;
using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using CsvHelper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Printing;
using System.Formats.Asn1;
using System.Threading;
using chdScoring.Contracts.Constants;
using chd.Base.Hosting.Forms.Base;
using chd.Base.Hosting.Contracts.Interfaces;
using chd.Base.Hosting.Forms.Extensions;
using chdScoring.Main.WebServer.Modules;
using chdScoring.Main.WebServer.Utils;
using chdScoring.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace chdScoring.Main.WebServer
{
    public partial class MainForm : BaseDiForm
    {
        private readonly IPrintCache _printCache;
        private readonly IDatabaseConfiguration _databaseConfiguration;
        private readonly IDeviceCacheService _deviceCacheService;

        public MainForm(IModuleManager moduleManager, IDeviceCacheService deviceCacheService, IPrintCache printCache, IServiceProvider serviceProvider, IDatabaseConfiguration databaseConfiguration)
        : base(moduleManager, serviceProvider)
        {
            this._databaseConfiguration = databaseConfiguration;
            this._printCache = printCache;
            this._deviceCacheService = deviceCacheService;

            InitializeComponent();
            var appSettings = this.GetService<IOptionsMonitor<AppSettings>>().CurrentValue;

            this.textBoxDefaultApiKey.Text = appSettings.ApiKey;
            this.textBoxRounPrintUrl.Text = appSettings.RoundScorePrintUrl;
            this.textBoxLocalApiUrl.Text = appSettings.LocalApiUrl;


            this._printCache.SetPrinter(PrinterSettings.InstalledPrinters[0]);

            this.comboBoxDataBase.DataSource = this._databaseConfiguration.GetConnections().Select(s => s.Name).ToList();

            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                this.comboBox1.Items.Add(printer);
            }

            this.comboBox1.SelectedValueChanged += this.ComboBox1_SelectedValueChanged;

            this.Resize += this.MainForm_Resize;
            this.checkBoxAutoPrint.Checked = this._printCache.AutoPrint;
            this._databaseConfiguration.ConnectionChanged += this._databaseConfiguration_ConnectionChanged;
            this._printCache.AutoPrintChanged += this._printCache_AutoPrintChanged;

            _deviceCacheService.DeviceStatusChanged += _deviceCacheService_DeviceStatusChanged;

            dataGridViewDevices.CellMouseClick += DataGridViewDevices_CellMouseClick;

        }

        private async void DataGridViewDevices_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0)
            {
                return;
            }

            // Zeile selektieren
            dataGridViewDevices.ClearSelection();
            dataGridViewDevices.Rows[e.RowIndex].Selected = true;
            dataGridViewDevices.CurrentCell = dataGridViewDevices.Rows[e.RowIndex].Cells[0];

            // Current aus dem Binding holen
            if (bindingSourceDevices.Current is not DeviceStatusDto item)
            {
                return;
            }

            await _moduleManager.HandleListContextMenu(item);
        }


        private void _deviceCacheService_DeviceStatusChanged(object? sender, EventArgs e)
        {
            this.TryInvoke(() => this.bindingSourceDevices.DataSource = _deviceCacheService.GetAll());
        }

        private void _printCache_AutoPrintChanged(object? sender, bool e)
        {
            if (e != this.checkBoxAutoPrint.Checked)
            {
                this.Invoke(() =>
                {
                    this.checkBoxAutoPrint.Checked = e;
                });
            }
        }

        private void ComboBox1_SelectedValueChanged(object? sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                this._printCache.SetPrinter(cb.SelectedItem.ToString());
            }
        }
        private void MainForm_Resize(object? sender, EventArgs e) => this.ShowHide(this.WindowState == FormWindowState.Minimized);

        private void schließenToolStripMenuItem_Click(object sender, EventArgs e)
        => this.Close();

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e) => this.ShowHide(false);

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e) => this.ShowHide(false);
        private void ShowHide(bool hide)
        {
            this.notifyIconMain.Visible = hide;
            if (hide)
            {
                this.Hide();
                this.notifyIconMain.ShowBalloonTip(2000, "chdScoring", "Webserver im Hintergrund", ToolTipIcon.Info);
            }
            else
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private async void comboBoxDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cB
                && cB.SelectedItem != this._databaseConfiguration.CurrentConnection)
            {
                this._databaseConfiguration.SetCurrentConnection(cB.SelectedItem.ToString());
            }
        }
        private async void _databaseConfiguration_ConnectionChanged(object? sender, string e)
        {
            this.TryInvoke(() =>
            {
                if (this.comboBoxDataBase.SelectedItem != e)
                {
                    this.comboBoxDataBase.SelectedItem = e;
                }
            });
            await this._moduleManager.GetModuleService<IFlightCacheService>(nameof(ServerModule)).Update(CancellationToken.None);
            await this._moduleManager.GetModuleService<IHubDataService>(nameof(ServerModule)).SendAll(CancellationToken.None);
        }

        private void checkBoxAutoPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (this._printCache.AutoPrint != this.checkBoxAutoPrint.Checked)
            {
                this._printCache.AutoPrint = this.checkBoxAutoPrint.Checked;
            }
        }


        private async void buttonImportCsv_Click(object sender, EventArgs e)
        {
            if (this.openFileDialogImportCsv.ShowDialog(this) != DialogResult.OK) { return; }
            using var fileStream = this.openFileDialogImportCsv.OpenFile();

            await _moduleManager.ImportPilots(this, fileStream, CancellationToken.None);
        }

        private void textBoxDefaultApiKey_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox tB)
            {
                _moduleManager.SetCustom<AppSettings, string>(x => x.ApiKey, tB.Text, nameof(ServerModule));
            }
        }

        private void textBoxRounPrintUrl_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox tB)
            {
                _moduleManager.SetCustom<AppSettings, string>(x => x.RoundScorePrintUrl, tB.Text, nameof(ServerModule));
            }
        }

        private void textBoxLocalApiUrl_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox tB)
            {
                _moduleManager.SetCustom<AppSettings, string>(x => x.LocalApiUrl, tB.Text, nameof(WebModule));
            }
        }
    }
}