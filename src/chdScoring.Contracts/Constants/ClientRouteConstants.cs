using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace chdScoring.Contracts.Constants
{
    public static class ClientRouteConstants
    {
        public const string Home = "";
        public const string Scoring = "scoring";
        public const string ApiKeys = "apikeys";
        public const string CompMgmt = "competitionmanagement";
        public const string ControlCenter = "controlcenter";
        public const string FCScoring = "fc-scoring";
        public const string Import = "import";
        public const string Pilots = "pilots";
        public const string ScoreCorrection = "correction";
        public const string Settings = "settings";
        public const string UIX = "uix";
        public const string Viewer = "timer";
        public const string DeviceControl = "devicecontrol";

        public static IEnumerable<(string Name, object? Value)> GetConstants()
        {
            return typeof(ClientRouteConstants)
                .GetFields(BindingFlags.Public |
                           BindingFlags.NonPublic |
                           BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => (f.Name, f.GetValue(null))).OrderBy(o => o.Name);
        }
    }
}
