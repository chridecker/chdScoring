using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using chdScoring.App.UI.Interfaces;

namespace chdScoring.App.WPF.Services
{
    public class FilePickerService : IFilePickerService
    {
        public Task<(string, byte[])> PickFileAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
