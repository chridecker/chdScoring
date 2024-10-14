using chdScoring.BusinessLogic.Services;

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
            this._apiLogger.LogAdded += this._apiLogger_LogAdded;
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
    }
}