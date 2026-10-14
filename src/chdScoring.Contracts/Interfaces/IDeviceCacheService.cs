using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDeviceCacheService
    {
        event EventHandler DeviceStatusChanged;

        bool AddConnection(string connectionId, string apiKey);
        bool SetDevice(string connectionId, DeviceStatusDto dto);
        DeviceStatusDto Get(string connectionId);
        List<DeviceStatusDto> GetAll();
        bool RemoveConnection(string connectionId);
    }
}
