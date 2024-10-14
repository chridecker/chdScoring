namespace chdScoring.Contracts.Constants
{
    public static class EndpointConstants
    {
        public const string ROOT = "chdScoring";

        public class Pilot
        {
            public const string ROUTE = "pilot";
            public const string GET_OpenRound = "openround";
            public const string GET_RoundResult = "roundresult";
            public const string POST_SetPilotActive = "setpilotactive";
            public const string POST_UnloadPilot = "unloadpilot";
        }
        public class Judge
        {
            public const string ROUTE = "judge";
            public const string GET_Flight = "flight";

        }
        public class Scoring
        {
            public const string ROUTE = "score";
            public const string POST_Save = "savescore";
            public const string POST_Update = "updatescore";

        }
        public class Control
        {
            public const string ROUTE = "control";

            public const string POST_TIMER = "timer";
            public const string POST_SaveRound = "saveround";

        }

    }
}
