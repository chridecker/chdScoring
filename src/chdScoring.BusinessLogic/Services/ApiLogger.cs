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
        private readonly ILogNotifyService _logNotifyService;

        public string Text => _log.ToString();

        public ApiLogger(ILogNotifyService logNotifyService)
        {
            this._logNotifyService = logNotifyService;
        }

        public Task Log(string message)
        {
            _log.AppendLine(message);
            this._logNotifyService.LogAdded();

            return Task.CompletedTask;
        }


    }
    public interface IApiLogger
    {
        string Text { get; }

        Task Log(string v);
    }
}
