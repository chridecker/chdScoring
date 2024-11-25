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
            public const string GET_All = "";
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

            public const string GET_OpenRound = "openround";
            public const string POST_TIMER = "timer";
            public const string POST_SaveRound = "saveround";
            public const string POST_CalcRound = "calcround";

        }

        public class Device
        {
            public const string ROUTE = "device";

            public const string GET = "";
            public const string GET_DeviceStatus = "device";
        }

        public class Database
        {
            public const string ROUTE = "database";
            public const string GET = "";
            public const string GET_CURRENT = "current";
            public const string POST_SETDATABASE = "set";
        }

        public class Print
        {
            public const string ROUTE = "print";
            public const string POST_ADD = "create";
            public const string GET_AUTOPRINT = "autoprint";
            public const string GET_PDF = "pdfs";
            public const string POST_CHANGE_AUTOPRINT = "changeautoprint";
            public const string POST_PRINT_PDF = "printpdf";
        }

    }
}
