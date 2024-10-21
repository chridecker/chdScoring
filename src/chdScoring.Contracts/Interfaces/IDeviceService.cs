using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceStatusDto>> GetAll(CancellationToken cancellationToken = default);
        Task<DeviceStatusDto> GetByName(string name, CancellationToken cancellationToken = default);
    }
}
