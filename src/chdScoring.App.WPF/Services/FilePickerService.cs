using chd.Base.UI.WPF.Hosting;
using chdScoring.App.UI.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace chdScoring.App.WPF.Services
{
    public class FilePickerService : IFilePickerService
    {
        private readonly IAppProvider _appProvider;

        public FilePickerService(IAppProvider appProvider)
        {
            this._appProvider = appProvider;
        }
        public async Task<(string, byte[])> PickFileAsync(CancellationToken cancellationToken)
        {
            var app = await this._appProvider.GetMainAppAsync();
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog(app.MainWindow);
            if (result ?? false)
            {
                var s = dialog.OpenFile();
                var ms = new MemoryStream();
                await s.CopyToAsync(ms);
                var x = ms.ToArray();

                return (new FileInfo(dialog.FileName).Name, x);
            }
            return (string.Empty, []);
        }
    }
}
