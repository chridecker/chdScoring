using chd.UI.Base.Client.Implementations.Authorization;
using chd.UI.Base.Contracts.Dtos.NavMenuNode;
using chd.UI.Base.Contracts.Interfaces.Authentication;
using chd.UI.Base.Contracts.Interfaces.NavMenu;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.UI.Services
{
    public class NavMenuService(ISettingManager settingManager) : INavMenuNodeService<int, int, int>
    {
        private List<NavMenuNodeDto<int, int>> _lst = [];

        public async Task<IList<NavMenuNodeDto<int, int>>> LoadNavMenuNodes(CancellationToken cancellationToken)
        {
            var home = CreateNavMenuNode(1, PageTitleConstants.Scoring, "scoring", "whistle", [RightConstants.AdminId, RightConstants.Judge]);

            var timer = CreateNavMenuNode(2, "Timer", "timer", "stopwatch", [RightConstants.AdminId, RightConstants.Viewer]);
            var cc = CreateNavMenuNode(3, "Control Center", "controlcenter", "calculator", [RightConstants.AdminId, RightConstants.Viewer]);

            var viewer = CreateNavFolderNode(4, "Viewer", "glasses", [RightConstants.AdminId, RightConstants.Viewer], timer, cc);

            var correction = CreateNavMenuNode(5, "Score Correction", "correction", "pen-to-square", [RightConstants.AdminId]);
            var compMgmt = CreateNavMenuNode(6, "Competition Management", "competitionmanagement", "stopwatch", [RightConstants.AdminId]);


            var pilots = CreateNavMenuNode(14, "Pilots", "pilots", "people-group", [RightConstants.AdminId]);
            var start = CreateNavMenuNode(15, "Startorder", "startorder", "arrow-down-1-9", [RightConstants.AdminId]);
            var pilotsFolder = CreateNavFolderNode(7, "Pilots", "pilots", [RightConstants.AdminId], pilots, start);

            var import = CreateNavMenuNode(8, "Import", "import", "file-import", [RightConstants.AdminId]);
            var fcScoreCorrection = CreateNavMenuNode(9, "FC-Score", "fc-scoring", "keyboard", [RightConstants.AdminId]);

            var fc = CreateNavFolderNode(10, "FlightCoach", "folder", [RightConstants.AdminId], import, fcScoreCorrection);

            var devices = CreateNavMenuNode(12, "Devices", "devicecontrol", "mobile", [RightConstants.AdminId]);
            var apiKeys = CreateNavMenuNode(13, "Api Keys", "apikeys", "key", [RightConstants.AdminId]);
            var maintenance = CreateNavFolderNode(11, "Maintenance", "folder", [RightConstants.AdminId], devices, apiKeys);

            _lst.Add(home);
            _lst.Add(viewer);
            _lst.Add(correction);
            _lst.Add(compMgmt);
            _lst.Add(pilotsFolder);
            _lst.Add(fc);
            _lst.Add(maintenance);

            return _lst;
        }

        public async Task<IList<NavMenuNodeDto<int, int>>> LoadNavMenuNodeFavorites(int userId,
            CancellationToken cancellationToken)
        {
            var favorites = await settingManager.GetSettingLocal<List<int>>(SettingConstants.UserNavFavorites) ?? [];
            return _lst.Flatten(n => n.SubMenuNodeLst).Where(x => favorites.Contains(x.Id)).ToList();
        }

        public async Task<bool> AddToFavorites(int id, int userId, CancellationToken cancellationToken = new CancellationToken())
        {
            var lst = await settingManager.GetSettingLocal<List<int>>(SettingConstants.UserNavFavorites) ?? [];
            if (!lst.Contains(id))
            {
                lst.Add(id);
                await settingManager.StoreSettingLocal(SettingConstants.UserNavFavorites, lst);
            }
            return true;
        }

        public async Task<bool> RemoveFromFavorites(int id, int userId, CancellationToken cancellationToken = new CancellationToken())
        {
            var lst = await settingManager.GetSettingLocal<List<int>>(SettingConstants.UserNavFavorites) ?? [];
            if (lst.Contains(id))
            {
                lst.Remove(id);
                await settingManager.StoreSettingLocal(SettingConstants.UserNavFavorites, lst);
            }
            return true;
        }


        private NavMenuNodeDto<int, int> CreateNavMenuNode(int id, string title, string url, string icon, int[] rights = null)
        {
            return new NavMenuNodeDto<int, int>
            {
                Id = id,
                Title = title,
                Link = url,
                FAIcon = icon,
                Rights = rights,
                NeedsAuthentication = true,
            };
        }
        private NavMenuNodeDto<int, int> CreateNavFolderNode(int id, string title, string icon,int[] rights = null, params NavMenuNodeDto<int, int>[] children)
        {
            var folder = new NavMenuNodeDto<int, int>
            {
                Id = id,
                Title = title,
                FAIcon = icon,
                SubMenuNodeLst = children,
                Rights = rights
            };
            foreach (var child in children)
            {
                child.ParentId = folder.Id;
            }

            return folder;
        }
    }
}
