using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using ManagedNativeWifi;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.WPF.Services
{
    public class NetworkManager : INetworkManager
    {
        public Task<List<WifiNetworkDto>> ScanNetworks(CancellationToken cancellationToken)
        {
            var ssids = NativeWifi.EnumerateAvailableNetworkSsids();
            return Task.FromResult(ssids.Select(s => new WifiNetworkDto { SSID = s.ToString() }).ToList());
        }

        public async Task<bool> ConnectToNetwork(string ssid, string password, CancellationToken cancellationToken)
        {
            var network = NativeWifi.EnumerateAvailableNetworks()
                .Where(x => x.ToString() == ssid)
                .OrderByDescending(o => o.SignalQuality)
                .FirstOrDefault();

            if (network is null)
            {
                return false;
            }

            return await NativeWifi.ConnectNetworkAsync(
                interfaceId: network.InterfaceInfo.Id,
                profileName: network.ProfileName,
                bssType: network.BssType,
                timeout: TimeSpan.FromSeconds(10));

        }

        public async Task<string> GetCurrentNetwork(CancellationToken cancellationToken)
        {
            var adapter = NativeWifi.EnumerateInterfaces().FirstOrDefault(x => x.State == InterfaceState.Connected);
            var c = NativeWifi.GetCurrentConnection(adapter.Id);
            return c.result is ActionResult.Success ? c.value.Ssid.ToString() : string.Empty;
        }
    }
}
