namespace chdScoring.Contracts.Constants
{
    public static class EndpointConstants
    {
        public const string ROOT = "chdScoring";

        public class ApiKey
        {
            public const string ROUTE = "apikey";
            public const string GETALL = "getall";
            public const string GET= "get";
            public const string SAVE = "save";
            public const string DELETE = "delete";
        }
        public class Authentication
        {
            public const string ROUTE = "authentication";
            public const string USER = "user";
            public const string USER_KEY = "userkey";
        }

        public class Device
        {
            public const string ROUTE = "device";
            public const string GETALL = "getall";
            public const string Navigate = "navigate";
            public const string Identify = "identify";
            public const string SetAutoRedirect = "setautoredirect";
            public const string SetApiKey = "setapikey";
            public const string SetZoom = "setzoom";
            public const string ToggleTheme = "ToggleTheme";
            public const string Shutdown = "shutdown";
        }

        public class Pilot
        {
            public const string ROUTE = "pilot";
            public const string GET_Img = "getimg";
            public const string GET_AllCountries = "allcountries";
            public const string GET_OpenRound = "openround";
            public const string GET_RoundResult = "roundresult";
            public const string GET_FinishedRounds = "finishedrounds";
            public const string POST_SetPilotActive = "setpilotactive";
            public const string POST_ChangeStartOrder = "changestartorder";
            public const string POST_UnloadPilot = "unloadpilot";
            public const string GET_Round = "round";
            public const string GET_All = "getall";
            public const string GET_Startorder = "getstartorder";
            public const string POST_AddNew = "AddNew";
            public const string DELETE_Delete = "Delete";
            public const string POST_Random= "Random";
            public const string POST_SetStart = "SetStart";
            public const string POST_Reflight = "Reflight";
            public const string POST_UPDATE_DATA = "updatedata";

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
            public const string POST_Confirm = "confirmscores";
            public const string POST_UnConfirm = "unconfirmscores";
            public const string POST_Update = "updatescore";

        }
        public class Control
        {
            public const string ROUTE = "control";

            public const string GET_OpenRound = "openround";
            public const string POST_TIMER = "timer";
            public const string POST_SaveRound = "saveround";
            public const string POST_CalcRound = "calcround";
            public const string POST_Start= "start";
            public const string DELETE_Clear = "clear";

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
            public const string POST_DELETE = "delete";
            public const string POST_ADD = "create";
            public const string GET_AUTOPRINT = "autoprint";
            public const string GET_PDF = "pdfs";
            public const string POST_CHANGE_AUTOPRINT = "changeautoprint";
            public const string POST_PRINT_PDF = "printpdf";
        }

        public class Import
        {
            public const string ROUTE = "import";
            public const string POST_JSONRESULT = "jsonresult";
        }

        public class Stammdaten
        {
            public const string ROUTE = "stammdaten";
            public const string GET_Image = "getimage";
            public const string GET_Stammdaten= "get";
            public const string GET_RoundResultPdfUrl= "getroundresultpdfurl";
        }
    }
}
