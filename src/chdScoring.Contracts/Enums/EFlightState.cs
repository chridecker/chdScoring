using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Enums
{
    public enum EFlightState : byte
    {
        Loaded = 0,
        OnAir = 1,
        Saved = 2,
        Calculated = 3
    }
}
