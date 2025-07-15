using chdScoring.Contracts.Enums;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface IKeyHandler
    {
        event EventHandler<EKeyInput> KeyInput;
        event EventHandler<KeyboardEventArgs> KeyDown;

        void InvokeKeyInput(EKeyInput key);
        Task OnKeyDown(KeyboardEventArgs key);
    }
}
