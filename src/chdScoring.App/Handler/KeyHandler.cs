using chdScoring.App.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace chdScoring.App.Handler
{
    public class KeyHandler : IKeyHandler
    {
        public event EventHandler<KeyboardEventArgs> KeyDown;

        [JSInvokable]
        public Task OnKeyDown(KeyboardEventArgs key) => Task.Run(() => this.KeyDown?.Invoke(this, key));

    }
    
}
