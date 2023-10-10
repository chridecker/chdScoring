using chdScoring.Client.Extensions;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading;

namespace chdScoring.Client.Services
{
    public class MainService : IMainService
    {
        private readonly HttpClient _client;
        private readonly ILogger<MainService> _logger;
        private readonly ISettingManager _settingManager;

        public MainService(ILogger<MainService> logger, IHttpClientFactory httpClientFactory, ISettingManager settingManager)
        {
            this._logger = logger;
            this._settingManager = settingManager;
            this._client = httpClientFactory.CreateClient<MainService>();
            this._client.DefaultRequestHeaders.Accept.Clear();
            this._client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IEnumerable<JudgeDto>> GetJudges(CancellationToken cancellationToken)
        {
            try
            {
                var uri = new UriBuilder(await this._settingManager.MainUrl).Uri;
                return await this._client.GetFromJsonAsync<IEnumerable<JudgeDto>>($"{uri}{EndpointConstants.ROUTE_judge}/", cancellationToken);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<JudgeDto>();
            }
        }

        public async Task<bool> TestConnection(CancellationToken cancellationToken)
        {
            try
            {
                var uri = new UriBuilder(await this._settingManager.MainUrl).Uri;
                var res = await this._client.GetAsync($"{uri}{EndpointConstants.GET_Test_Connection}/", cancellationToken);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CurrentFlight> GetCurrentFlight(CancellationToken cancellationToken)
        {
            try
            {
                var uri = new UriBuilder(await this._settingManager.MainUrl).Uri;
                return await this._client.GetFromJsonAsync<CurrentFlight>($"{uri}{EndpointConstants.ROUTE_judge}/{EndpointConstants.GET_Flight}", cancellationToken);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> SaveScore(int id, int figur, int judge, int round, decimal value, CancellationToken token)
        {
            try
            {
                var dto = new SaveScoreDto
                {
                    Pilot = id,
                    Figur = figur,
                    Judge = judge,
                    Round = round,
                    Value = value
                };
                var uri = new UriBuilder(await this._settingManager.MainUrl).Uri;
                var res  = await this._client.PostAsJsonAsync($"{uri}{EndpointConstants.ROUTE_Scoring}/{EndpointConstants.POST_Save}", dto, cancellationToken: token);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
    public interface IMainService
    {
        Task<bool> TestConnection(CancellationToken cancellationToken);
        Task<IEnumerable<JudgeDto>> GetJudges(CancellationToken cancellationToken);
        Task<CurrentFlight> GetCurrentFlight(CancellationToken cancellationToken);
        Task<bool> SaveScore(int id, int figur, int judge, int round, decimal value, CancellationToken token);
    }
}
