using chd.Api.Base.Client;
using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Main.Client.Clients
{
    public class DeviceClient : BaseApiService, IDeviceService
    {
        public DeviceClient(ILogger<DeviceClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<IEnumerable<DeviceStatusDto>> GetAll(CancellationToken cancellationToken = default)
        => this.Get<IEnumerable<DeviceStatusDto>>(EndpointConstants.Device.GET,cancellationToken);

        public Task<DeviceStatusDto> GetByName(string name, CancellationToken cancellationToken = default)
        => this.Get<DeviceStatusDto>(EndpointConstants.Judge.GET_Flight.SetUrlParameters((nameof(name),name)), cancellationToken);

      
    }
}
