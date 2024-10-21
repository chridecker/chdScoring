using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDeviceStatusCache : IDeviceService
    {
        void Remove(string connectionId);

        void UpdateDto(string connectionId,  DeviceStatusDto dto);
    }
}
