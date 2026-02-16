using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Printing;
using System.Formats.Asn1;
using System.Threading;

namespace chdScoring.Main.WebServer
{
    public partial class MainForm : Form
    {
        private readonly IApiLogger _apiLogger;
        private readonly IPrintCache _printCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseConfiguration _databaseConfiguration;

        public MainForm(IApiLogger apiLogger, IPrintCache printCache, IServiceProvider serviceProvider, IDatabaseConfiguration databaseConfiguration)
        {
            this._databaseConfiguration = databaseConfiguration;
            this._serviceProvider = serviceProvider;
            this._printCache = printCache;

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
            using var reader = new CsvReader(new StreamReader(fileStream), System.Globalization.CultureInfo.InvariantCulture);

            using var scope = this._serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IPilotService>();
            var pilots = await service.GetAllPilots(CancellationToken.None);
            var countries = await service.GetCountries(CancellationToken.None);
            var pilotsCount = pilots.Max(s => s.Id);

            try
            {
                await reader.ReadAsync();
                reader.ReadHeader();
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