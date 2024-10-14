using Microsoft.AspNetCore.Components.Web;

namespace chdScoring.App.UI.Interfaces
{
    public interface IKeyHandler
    {
        event EventHandler<KeyboardEventArgs> KeyDown;
        Task OnKeyDown(KeyboardEventArgs key);
    }
}
