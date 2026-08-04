using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.Services
{
    public class FilePickerService : IFilePickerService
    {
        private readonly IFilePicker _filePicker;
        public FilePickerService()
        {
            _filePicker = FilePicker.Default;
        }
        public async Task<byte[]> PickFileAsync(CancellationToken cancellationToken)
        {
            var result = await _filePicker.PickAsync();
            if (result != null)
            {
                var s = await result.OpenReadAsync();
                var ms = new MemoryStream();
                await s.CopyToAsync(ms);
                var x = ms.ToArray();
                return x;
            }

            return null;
        }
    }
}
