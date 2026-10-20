using System;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class ApiLogger : IApiLogger
    {
        private readonly StringBuilder _log = new();

        public event EventHandler LogAdded;
        public string Text => _log.ToString();

        public bool LogDetail { get; set; }

        public void Clear()
        {
            this._log.Clear();
            this.LogAdded?.Invoke(this, EventArgs.Empty);
        }

        public Task Log(string message, string details = null)
        {
            _log.AppendLine($"{DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy")}: {message}");
            if (this.LogDetail && !string.IsNullOrWhiteSpace(details))
            {
                _log.AppendLine($"\r\r{details}");
            }
            this.LogAdded?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }


    }
    public interface IApiLogger
    {
        string Text { get; }
        Task Log(string message, string details = null);
        void Clear();
        event EventHandler LogAdded;
        bool LogDetail { get; set; }
    }
}
