using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.UI.Interfaces
{
    public interface INetworkManager
    {
        Task<List<WifiNetworkDto>> ScanNetworks(CancellationToken cancellationToken);
        Task<bool> ConnectToNetwork(string ssid, string password, CancellationToken cancellationToken);
        Task<string> GetCurrentNetwork(CancellationToken cancellationToken);
    }
}
