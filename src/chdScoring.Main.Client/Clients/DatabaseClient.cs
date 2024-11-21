using chd.Api.Base.Client;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Clients
{
    public class DatabaseClient : BaseApiService, IDatabaseService
    {
        public DatabaseClient(ILogger<BaseApiService> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public async Task<string> GetCurrentDatabaseConnection(CancellationToken cancellationToken = default)
        => await this.Get<string>(Database.GET_CURRENT, cancellationToken);

        public async Task<IEnumerable<string>> GetDatabaseConnections(CancellationToken cancellationToken = default)
         => await this.Get<IEnumerable<string>>(Database.GET, cancellationToken);

        public async Task<bool> SetDatabaseConnection(string name, CancellationToken cancellationToken = default)
        => await this.Post<bool>(Database.POST_SETDATABASE, new SetDatabaseConnectionDto { ConnectionName = name }, cancellationToken);
    }
}
