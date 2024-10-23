using chd.UI.Base.Contracts.Interfaces.Authentication;
using chd.UI.Base.Contracts.Interfaces.Update;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Services
{
    public class DeviceStatusService : IDeviceStatusService
    {
        private readonly IBatteryService _batteryService;
        private readonly IUpdateService _updateService;
        private readonly IchdScoringProfileService _profileService;

        public DeviceStatusService(IBatteryService batteryService, IUpdateService updateService, IchdScoringProfileService ichdScoringProfileService)
        {
            this._batteryService = batteryService;
            this._updateService = updateService;
            this._profileService = ichdScoringProfileService;
        }
        public async Task<DeviceStatusDto> GetStatus(CancellationToken cancellationToken = default)
        => new DeviceStatusDto
        {
            BatteryLevel = this._batteryService.BatteryLevel,
            BatteryCharging = this._batteryService.Charging,
            HostName = this._batteryService.DeviceName,
            CurrentVersion = await this._updateService.CurrentVersion(),
            CurrentUser = this._profileService.CsUser,
            CurrentLocalTime = DateTime.Now,
        };
    }
}
