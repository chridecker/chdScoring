using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.UI.Interfaces
{
    public interface IFilePickerService
    {
        Task<byte[]> PickFileAsync(CancellationToken cancellationToken);
    }
}
