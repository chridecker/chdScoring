using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class DeviceStatusCache : IDeviceStatusCache
    {
        private ConcurrentDictionary<string, DeviceStatusDto> _deviceStatusDict = new ConcurrentDictionary<string, DeviceStatusDto>();

        public DeviceStatusCache()
        {

        }

        public Task<IEnumerable<DeviceStatusDto>> GetAll(CancellationToken cancellationToken = default) => Task.FromResult(this._deviceStatusDict.Values.Where(x => x != null));
        public Task<DeviceStatusDto> GetByName(string name, CancellationToken cancellationToken = default) => Task.FromResult(this._deviceStatusDict.Values.FirstOrDefault(x => x?.HostName == name));

        public void Remove(string connectionId) => this._deviceStatusDict.TryRemove(connectionId, out _);


        public void UpdateDto(string connectionId, DeviceStatusDto dto)
        {
            this._deviceStatusDict[connectionId] = dto;
        }
    }
}
