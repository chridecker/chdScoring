using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IApiKeyHandler
    {
        Task<string> ApiKey();
    }
}
