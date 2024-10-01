using chdScoring.PrintService.Services;
using System.Drawing.Printing;

namespace chdScoring.PrintService
{
    public partial class PrintForm : Form
    {
        private readonly IPrintCache _printCache;

        public PrintForm(IPrintCache printCache)
        {
            this._printCache = printCache;

            InitializeComponent();
            this._printCache.SetPrinter(PrinterSettings.InstalledPrinters[0]);
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                this.comboBox1.Items.Add(printer);
            }

            this.comboBox1.SelectedValueChanged += this.ComboBox1_SelectedValueChanged;
        }

        private void ComboBox1_SelectedValueChanged(object? sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                this._printCache.SetPrinter(cb.SelectedItem.ToString());
            }
        }
    }
}
