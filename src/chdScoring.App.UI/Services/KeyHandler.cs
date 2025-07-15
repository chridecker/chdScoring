using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Enums;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Services
{
    public class KeyHandler : IKeyHandler
    {
        public event EventHandler<EKeyInput> KeyInput;

        public void InvokeKeyInput(EKeyInput key) => this.KeyInput?.Invoke(this, key);


        public event EventHandler<KeyboardEventArgs> KeyDown;

        [JSInvokable]
        public Task OnKeyDown(KeyboardEventArgs key) => Task.Run(() => this.KeyDown?.Invoke(this, key));
    }
}
