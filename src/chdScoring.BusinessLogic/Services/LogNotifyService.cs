using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class LogNotifyService : ILogNotifyService
    {
        public event EventHandler LogUpdate;

        public void LogAdded()
        {
            this.LogUpdate?.Invoke(this,EventArgs.Empty);
        }
    }
    public interface ILogNotifyService
    {
        event EventHandler LogUpdate;

        void LogAdded();
    }
}
