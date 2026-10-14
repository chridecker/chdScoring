using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class DeviceStatusDto
    {
        public double BatteryLevel { get; set; }
        public bool? BatteryCharging { get; set; }

        public string ConnectionId { get; set; }
        public string ApiKey { get; set; }
        public string User { get; set; }
        public string HostName { get; set; }
        public string CurrentSite { get; set; }
        public Version CurrentVersion { get; set; }
        public DateTime CurrentLocalTime { get; set; }
        public DateTime? LastUpdate { get; set; }


        public override bool Equals(object obj)
        => obj is DeviceStatusDto dto && dto.ConnectionId == this.ConnectionId;
    }
}
