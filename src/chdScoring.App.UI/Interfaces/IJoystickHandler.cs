using chdScoring.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface IJoystickHandler
    {
        event EventHandler<EJoystickMotionDirection> Motion;
        void InvokeMotion(EJoystickMotionDirection motion);
    }
}
