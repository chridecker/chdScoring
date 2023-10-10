using chdScoring.BusinessLogic.Services;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace chdScoring.Main.UI
{
    public partial class MainForm : Form
    {
        private readonly IApiLogger _apiLogger;
        private volatile bool _closing = true;
        public MainForm(IApiLogger apiLogger)
        {
            InitializeComponent();

            this.Resize += this.MainForm_Resize;
            this._apiLogger = apiLogger;
            this._apiLogger.LogAdded += this._apiLogger_LogAdded;
        }

        private void _apiLogger_LogAdded(object? sender, EventArgs e)
        {
            this.textBoxWebLog.Text = this._apiLogger.Text;
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIconMain.ShowBalloonTip(2000);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = this._closing;
            this.Hide();
            this.WindowState = FormWindowState.Minimized;

            base.OnFormClosing(e);
        }

        private void schlieﬂenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._closing = false;
            this.Close();
            Application.Exit();
        }

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._apiLogger.Clear();
        }
    }
}