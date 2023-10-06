using chdScoring.DataAccess.Contracts.Repositories;

namespace chdScoring.Main.UI
{
    public partial class MainForm : Form
    {
        private readonly ITeilnehmerRepository _teilnehmerRepository;

        public MainForm(ITeilnehmerRepository teilnehmerRepository)
        {
            InitializeComponent();
            this._teilnehmerRepository = teilnehmerRepository;
        }
    }
}