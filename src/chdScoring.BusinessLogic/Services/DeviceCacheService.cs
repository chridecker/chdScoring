using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using chdScoring.Contracts.Dtos;

namespace chdScoring.BusinessLogic.Services
{
    public class DeviceCacheService : IDeviceCacheService
    {
        private readonly ConcurrentDictionary<string, DeviceStatusDto> _devices = [];

        public event EventHandler DeviceStatusChanged;

        public bool AddConnection(string connectionId, string apiKey)
        {
            var added = this._devices.TryAdd(connectionId, new DeviceStatusDto { ApiKey = apiKey, ConnectionId = connectionId });
            this.DeviceStatusChanged?.Invoke(this, EventArgs.Empty);
            return added;
        }

        public bool SetDevice(string connectionId, DeviceStatusDto dto)
        {
            var updated = this._devices.TryUpdate(connectionId, dto, this._devices[connectionId]);
            this.DeviceStatusChanged?.Invoke(this, EventArgs.Empty);
            return updated;
        }

        public DeviceStatusDto Get(string connectionId) => this._devices.TryGetValue(connectionId, out DeviceStatusDto device) ? device : null;

        public List<DeviceStatusDto> GetAll() => this._devices.Values.OrderBy(o => o.LastUpdate).ToList();

        public bool RemoveConnection(string connectionId)
        {
            var removed = this._devices.TryRemove(connectionId, out _);
            this.DeviceStatusChanged?.Invoke(this, EventArgs.Empty);
            return removed;
        }
    }
}
