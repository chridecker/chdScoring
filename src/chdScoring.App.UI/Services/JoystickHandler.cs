using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Services
{
    public class JoystickHandler : IJoystickHandler
    {
        public event EventHandler<EJoystickMotionDirection> Motion;

        public void InvokeMotion(EJoystickMotionDirection motion)
            => this.Motion?.Invoke(this, motion);
    }
}
