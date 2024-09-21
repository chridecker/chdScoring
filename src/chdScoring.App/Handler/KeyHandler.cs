using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Handler
{
    public class KeyHandler : IKeyHandler
    {
        public event EventHandler<KeyboardEventArgs> KeyDown;

        [JSInvokable]
        public Task OnKeyDown(KeyboardEventArgs key) => Task.Run(() => this.KeyDown?.Invoke(this, key));

    }
    public interface IKeyHandler
    {
        event EventHandler<KeyboardEventArgs> KeyDown;
        Task OnKeyDown(KeyboardEventArgs key);
    }
}
