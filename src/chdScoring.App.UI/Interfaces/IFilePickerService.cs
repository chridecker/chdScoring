using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.UI.Interfaces
{
    public interface IFilePickerService
    {
        Task<(string,byte[])> PickFileAsync(CancellationToken cancellationToken);
    }
}
