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

namespace chdScoring.Main.WebServer
{
    public partial class MainForm : Form
    {
        private readonly IApiLogger _apiLogger;
        private readonly IPrintCache _printCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseConfiguration _databaseConfiguration;
        private readonly IDeviceCacheService _deviceCacheService;

        public MainForm(IApiLogger apiLogger, IDeviceCacheService deviceCacheService, IPrintCache printCache, IServiceProvider serviceProvider, IDatabaseConfiguration databaseConfiguration)
        {
            this._databaseConfiguration = databaseConfiguration;
            this._serviceProvider = serviceProvider;
            this._printCache = printCache;
            this._deviceCacheService = deviceCacheService;

            InitializeComponent();

            this._printCache.SetPrinter(PrinterSettings.InstalledPrinters[0]);

            this.comboBoxDataBase.DataSource = this._databaseConfiguration.GetConnections().Select(s => s.Name).ToList();

            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                this.comboBox1.Items.Add(printer);
            }

            this.comboBox1.SelectedValueChanged += this.ComboBox1_SelectedValueChanged;

            this.Resize += this.MainForm_Resize;
            this._apiLogger = apiLogger;
            this._apiLogger.LogAdded += this._apiLogger_LogAdded;
            this.checkBoxAutoPrint.Checked = this._printCache.AutoPrint;
            this._databaseConfiguration.ConnectionChanged += this._databaseConfiguration_ConnectionChanged;
            this._printCache.AutoPrintChanged += this._printCache_AutoPrintChanged;

            _deviceCacheService.DeviceStatusChanged += _deviceCacheService_DeviceStatusChanged;

            dataGridViewDevices.CellMouseClick += DataGridViewDevices_CellMouseClick;

        }

        //private async void BindingSourceApiKeys_CurrentChanged(object? sender, EventArgs e)
        //{
        //    if (sender is BindingSource bs
        //       && bs.Current is ApiKeyDto apiKey)
        //    {
        //        using var scope = _serviceProvider.CreateScope();
        //        var svc = scope.ServiceProvider.GetService<IApiKeyService>();
        //        await svc.SaveAsync(apiKey, CancellationToken.None);
        //    }
        //}

        protected override async void OnShown(EventArgs e)
        {
            await ReloadApiKeys(CancellationToken.None);
            base.OnShown(e);
            //bindingSourceApiKeys.CurrentItemChanged += BindingSourceApiKeys_CurrentChanged;
        }

        private async void DataGridViewDevices_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0)
                return;

            // Zeile selektieren
            dataGridViewDevices.ClearSelection();
            dataGridViewDevices.Rows[e.RowIndex].Selected = true;
            dataGridViewDevices.CurrentCell = dataGridViewDevices.Rows[e.RowIndex].Cells[0];

            // Current aus dem Binding holen
            if (bindingSourceDevices.Current is not DeviceStatusDto item)
                return;

            var menu = new ContextMenuStrip();

            var gotToMenu = new ToolStripMenuItem("Go To");
            foreach (var route in ClientRouteConstants.GetConstants())
            {
                gotToMenu.DropDownItems.Add(route.Name, null, async (_, _) =>
                {
                    await SendRedirect(item, route.Value.ToString());
                });
            }
            menu.Items.Add(gotToMenu);

            var autoRedirectMenu = new ToolStripMenuItem("Auto Redirect");
            foreach (var route in ClientRouteConstants.GetConstants())
            {
                autoRedirectMenu.DropDownItems.Add(route.Name, null, async (_, _) =>
                {
                    await SendAutoRedirect(item, route.Value.ToString());
                });
            }
            autoRedirectMenu.DropDownItems.Add("Clear", null, async (_, _) =>
            {
                await SendAutoRedirect(item, string.Empty);
            });

            menu.Items.Add(autoRedirectMenu);

            var setZoomMenu = new ToolStripMenuItem("Zoom");

            foreach (var val in new List<int> { 50, 60, 75, 80, 90, 100, 110, 125, 150, 200 }) // 50 to 200
            {
                setZoomMenu.DropDownItems.Add($"{val}%", null, async (_, _) =>
                {
                    await SetZoom(item, val);
                });
            }

            menu.Items.Add(setZoomMenu);


            menu.Items.Add("Beep", null, async (_, _) =>
            {
                await SendIdentify(item);
            });

            menu.Items.Add("Restart", null, async (_, _) =>
            {
                await SendShutdown(item, true);
            });



            menu.Show(Cursor.Position);
        }

        private Task SendIdentify(DeviceStatusDto dto)
            => SendDeviceHub(async c =>
            {
                await c.Identify(dto);
            }, dto);

        private Task SendShutdown(DeviceStatusDto dto, bool restart)
            => SendDeviceHub(async c =>
            {
                await c.ShutDown(dto, restart);
            }, dto);
        private Task SendRedirect(DeviceStatusDto dto, string route)
            => SendDeviceHub(async c =>
            {
                await c.NavigateTo(dto, route);
            }, dto);

        private Task SendAutoRedirect(DeviceStatusDto dto, string route)
            => SendDeviceHub(async c =>
            {
                await c.SetAutoRedirect(dto, route);
            }, dto);

        private Task SetZoom(DeviceStatusDto dto, int zoom)
            => SendDeviceHub(async c =>
            {
                await c.SetZoom(dto, zoom);
            }, dto);
        private async Task SendDeviceHub(Func<IDeviceHub, Task> action, DeviceStatusDto dto)
        {
            try
            {
                using var scope = this._serviceProvider.CreateScope();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<DeviceHub, IDeviceHub>>();
                await action(hubContext.Clients.Client(dto.ConnectionId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _deviceCacheService_DeviceStatusChanged(object? sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => this.bindingSourceDevices.DataSource = _deviceCacheService.GetAll());
            }
            else
            {
                this.bindingSourceDevices.DataSource = _deviceCacheService.GetAll();
            }
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

        private void _apiLogger_LogAdded(object? sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => this.textBoxWebLog.Text = this._apiLogger.Text);
            }
            else
            {
                this.textBoxWebLog.Text = this._apiLogger.Text;
            }
        }

        private void MainForm_Resize(object? sender, EventArgs e) => this.ShowHide(this.WindowState == FormWindowState.Minimized);

        private void schließenToolStripMenuItem_Click(object sender, EventArgs e)
        => this.Close();

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e) => this.ShowHide(false);

        private void button1_Click(object sender, EventArgs e)
        {
            this._apiLogger.Clear();
        }

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
            this.Invoke(() =>
            {
                if (this.comboBoxDataBase.SelectedItem != e)
                {
                    this.comboBoxDataBase.SelectedItem = e;
                }
            });
            using var scope = this._serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IFlightCacheService>().Update(CancellationToken.None);
            await scope.ServiceProvider.GetService<IHubDataService>().SendAll(CancellationToken.None);

            await ReloadApiKeys(CancellationToken.None);
        }

        private void checkBoxAutoPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (this._printCache.AutoPrint != this.checkBoxAutoPrint.Checked)
            {
                this._printCache.AutoPrint = this.checkBoxAutoPrint.Checked;
            }
        }

        private async Task ReloadApiKeys(CancellationToken cancellationToken)
        {
            using var scope = this._serviceProvider.CreateScope();
            var svc = scope.ServiceProvider.GetService<IApiKeyService>();

            if (this.InvokeRequired)
            {
                await this.InvokeAsync(async () => bindingSourceApiKeys.DataSource = await svc.GetAllAsync(cancellationToken), cancellationToken);
            }
            else
            {
                bindingSourceApiKeys.DataSource = await svc.GetAllAsync(cancellationToken);
            }


        }


        private async void buttonImportCsv_Click(object sender, EventArgs e)
        {
            if (this.openFileDialogImportCsv.ShowDialog(this) != DialogResult.OK) { return; }
            using var fileStream = this.openFileDialogImportCsv.OpenFile();
            using var reader = new CsvReader(new StreamReader(fileStream), System.Globalization.CultureInfo.InvariantCulture);

            using var scope = this._serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IPilotService>();
            var pilots = await service.GetAllPilots(CancellationToken.None);
            var countries = await service.GetCountries(CancellationToken.None);
            var pilotsCount = pilots.Any() ? pilots.Max(s => s.Id) : 0;

            try
            {
                while (await reader.ReadAsync())
                {
                    var name = reader.GetField(1);
                    var lastname = reader.GetField(2);
                    var club = reader.GetField(4);
                    var license = reader.GetField(5);
                    var country = reader.GetField(3);


                    if (!await service.Add(new Contracts.Dtos.PilotDto
                    {
                        Id = ++pilotsCount,
                        Firstname = name,
                        Lastname = lastname,
                        Club = club,
                        License = license,
                        CountryId = countries.FirstOrDefault(c => c.Name == country)?.Id ?? 0,
                    }, CancellationToken.None))
                    {
                        MessageBox.Show(this, $"Fehler beim Importieren der CSV: Pilot {name} {lastname} konnte nicht hinzugefügt werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Fehler beim Importieren der CSV: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}