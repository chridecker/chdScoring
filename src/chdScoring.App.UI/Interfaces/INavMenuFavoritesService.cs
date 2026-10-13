using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.UI.Interfaces
{
    public interface INavMenuFavoritesService
    {
        Task<List<T>> GetAsync<T>(CancellationToken cancellationToken) where T : struct;
        Task<bool> AddAsync<T>(T id, CancellationToken cancellationToken) where T : struct;
        Task<bool> RemoveAsync<T>(T id, CancellationToken cancellationToken) where T : struct;
    }
}
