using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Enums;
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
    }
}
