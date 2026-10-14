using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDeviceHub
    {
        Task RegisterDeviceAsync(DeviceStatusDto dto);
    }
}
