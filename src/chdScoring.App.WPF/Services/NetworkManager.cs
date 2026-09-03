using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;

namespace chdScoring.App.WPF.Services
{
    public class NetworkManager : INetworkManager
    {
        public Task<List<WifiNetworkDto>> ScanNetworks(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConnectToNetwork(string ssid, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCurrentNetwork(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
