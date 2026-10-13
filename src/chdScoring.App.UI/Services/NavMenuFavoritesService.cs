using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.App.UI.Constants;

namespace chdScoring.App.UI.Services
{
    public class NavMenuFavoritesService(IchdScoringProfileService profileService, ISettingManager settingManager) : INavMenuFavoritesService
    {
        public async Task<List<T>> GetAsync<T>(CancellationToken cancellationToken) where T : struct
        {
            if (profileService.CsUser is null) { return []; }
            return await settingManager.GetSettingLocal<List<T>>(SettingConstants.UserNavFavorites) ?? [];
        }

        public async Task<bool> AddAsync<T>(T id, CancellationToken cancellationToken) where T : struct
        {
            if (profileService.CsUser is null) { return false; }
            var lst = await settingManager.GetSettingLocal<List<T>>(SettingConstants.UserNavFavorites) ?? [];
            if (!lst.Contains(id))
            {
                lst.Add(id);
                await settingManager.StoreSettingLocal(SettingConstants.UserNavFavorites, lst);
            }
            return true;
        }

        public async Task<bool> RemoveAsync<T>(T id, CancellationToken cancellationToken) where T : struct
        {
            if (profileService.CsUser is null) { return false; }
            var lst = await settingManager.GetSettingLocal<List<T>>(SettingConstants.UserNavFavorites) ?? [];
            if (lst.Contains(id))
            {
                lst.Remove(id);
                await settingManager.StoreSettingLocal(SettingConstants.UserNavFavorites, lst);
            }
            return true;
        }
    }
}
