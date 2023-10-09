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

        public MainForm(IApiLogger apiLogger)
        {
            InitializeComponent();

            this.Resize += this.MainForm_Resize;
            this._apiLogger = apiLogger;
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
            e.Cancel = true;
            this.Hide();
            this.WindowState = FormWindowState.Minimized;

            base.OnFormClosing(e);
        }

        private void schlieﬂenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;

        }
    }
}