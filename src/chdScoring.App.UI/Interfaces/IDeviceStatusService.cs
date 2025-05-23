﻿using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface IDeviceStatusService
    {
        Task<DeviceStatusDto> GetStatus(CancellationToken cancellationToken = default);
    }
}
