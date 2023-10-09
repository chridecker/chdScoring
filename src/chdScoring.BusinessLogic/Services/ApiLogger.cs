using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class ApiLogger : IApiLogger
    {
        private readonly StringBuilder _log = new StringBuilder();

        public event EventHandler LogAdded;
        public string Text => _log.ToString();



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
        event EventHandler LogAdded;
    }
}
