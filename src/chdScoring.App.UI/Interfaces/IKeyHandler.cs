using chdScoring.Contracts.Enums;
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
        void InvokeKeyInput(EKeyInput key);
    }
}
