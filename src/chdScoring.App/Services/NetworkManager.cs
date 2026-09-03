using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using MauiWifiManager;
using MauiWifiManager.Abstractions;

namespace chdScoring.App.Services
{
    public class NetworkManager : INetworkManager
    {
        private readonly IWifiNetworkService _wifiManager;
        public NetworkManager()
        {
            _wifiManager = CrossWifiManager.Current;
        }

        public async Task<List<WifiNetworkDto>> ScanNetworks(CancellationToken cancellationToken)
        {
            var networks = await this._wifiManager.ScanWifiNetworksAsync(cancellationToken);
            if(networks.ErrorCode is not WifiErrorCodes.Success)
            {
                throw new InvalidOperationException(networks.ErrorMessage);
            }
            return networks.Data.Select(n => new WifiNetworkDto
            {
                SSID = n.Ssid
            }).ToList();
        }

        public async Task<bool> ConnectToNetwork(string ssid, string password, CancellationToken cancellationToken)
        {
            var current = await this._wifiManager.GetNetworkInfoAsync();
            if (current.ErrorCode is WifiErrorCodes.Success && current.Data.Ssid == ssid)
            {
                return true;
            }

            var result = await this._wifiManager.ConnectWifiAsync(ssid, password, cancellationToken);
            if (result.ErrorCode is not WifiErrorCodes.Success)
            {
                throw new InvalidOperationException(result.ErrorMessage);
            }
            return result.Data.IpAddress != 0;
        }

        public async Task<string> GetCurrentNetwork(CancellationToken cancellationToken)
        {
            var result = await this._wifiManager.GetNetworkInfoAsync(cancellationToken);
            if (result.ErrorCode is not WifiErrorCodes.Success)
            {
                throw new InvalidOperationException(result.ErrorMessage);
            }
            return result.Data.Ssid;
            // Do something with the current network information
        }
    }
}
