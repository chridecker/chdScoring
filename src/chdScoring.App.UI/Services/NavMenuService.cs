using chd.UI.Base.Contracts.Dtos.NavMenuNode;
using chd.UI.Base.Contracts.Interfaces.NavMenu;
using chdScoring.App.UI.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Constants;

namespace chdScoring.App.UI.Services
{
    public class NavMenuService : INavMenuNodeService<int, int, int>
    {
        public async Task<IList<NavMenuNodeDto<int, int>>> LoadNavMenuNodes(CancellationToken cancellationToken)
        {
            var menuLst = new List<NavMenuNodeDto<int, int>>();
            var home = CreateNavMenuNode(1, PageTitleConstants.Scoring, "scoring", "whistle", [RightConstants.AdminId, RightConstants.Judge]);

            var timer = CreateNavMenuNode(2, "Timer", "timer", "stopwatch", [RightConstants.AdminId, RightConstants.Viewer]);
            var cc = CreateNavMenuNode(3, "Control Center", "controlcenter", "calculator", [RightConstants.AdminId, RightConstants.Viewer]);

            var viewer = CreateNavFolderNode(4, "Viewer", "glasses", timer, cc);

            var correction = CreateNavMenuNode(4, "Score Correction", "correction", "pen-to-square", [RightConstants.AdminId]);
            var compMgmt = CreateNavMenuNode(4, "Competition Management", "competitionmanagement", "stopwatch", [RightConstants.AdminId]);
            var pilots = CreateNavMenuNode(4, "Pilots", "pilots", "people-group", [RightConstants.AdminId]);

            var import = CreateNavMenuNode(7, "Import", "import", "file-import", [RightConstants.AdminId]);
            var fcScoreCorrection = CreateNavMenuNode(8, "FC-Score", "fc-scoring", "keyboard", [RightConstants.AdminId]);

            var fc = CreateNavFolderNode(10, "FlightCoach", "folder", import, fcScoreCorrection);


            menuLst.Add(home);
            menuLst.Add(viewer);
            menuLst.Add(correction);
            menuLst.Add(compMgmt);
            menuLst.Add(pilots);
            menuLst.Add(fc);

            return menuLst;
        }

        public async Task<IList<NavMenuNodeDto<int, int>>> LoadNavMenuNodeFavorites(int userId, CancellationToken cancellationToken) => [];

        public Task<bool> AddToFavorites<T>(T id, int userId, CancellationToken cancellationToken = new CancellationToken())
            => Task.FromResult(false);

        public Task<bool> RemoveFromFavorites<T>(T id, int userId, CancellationToken cancellationToken = new CancellationToken())
            => Task.FromResult(false);

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
        private NavMenuNodeDto<int, int> CreateNavFolderNode(int id, string title, string icon, params NavMenuNodeDto<int, int>[] children)
        {
            var folder = new NavMenuNodeDto<int, int>
            {
                Id = id,
                Title = title,
                FAIcon = icon,
                SubMenuNodeLst = children
            };
            foreach (var child in children)
            {
                child.ParentId = folder.Id;
            }

            return folder;
        }
    }
}
