using System;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class ApiLogger : IApiLogger
    {
        private readonly StringBuilder _log = new StringBuilder();

        public event EventHandler LogAdded;
        public string Text => _log.ToString();

        public void Clear()
        {
            this._log.Clear();
            this.LogAdded?.Invoke(this, EventArgs.Empty);
        }

        public Task Log(string message)
        {
            _log.AppendLine(message);
            this.LogAdded?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }


    }
    public interface IApiLogger
    {
        string Text { get; }

        Task Log(string v);
        void Clear();

        event EventHandler LogAdded;
    }
}
