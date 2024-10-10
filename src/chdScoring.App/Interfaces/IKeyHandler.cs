using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Interfaces
{
    public interface IKeyHandler
    {
        event EventHandler<KeyboardEventArgs> KeyDown;
        Task OnKeyDown(KeyboardEventArgs key);
    }
}
