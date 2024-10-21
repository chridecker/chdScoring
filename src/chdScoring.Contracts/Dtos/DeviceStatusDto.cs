using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class DeviceStatusDto
    {
        public double BatteryLevel { get; set; }
        public bool? BatteryCharging{ get; set; }
        public csUserDto CurrentUser { get; set; }
        public string HostName { get; set; }
        public Version CurrentVersion { get; set; }
        public DateTime CurrentLocalTime { get; set; }
        public DateTime? LastUpdate { get; set; }

    }
}
