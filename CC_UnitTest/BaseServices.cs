namespace CC_UnitTest
{
    public class BaseServices
    {
        internal const string _baseUrl = "https://localhost:44326";

        //public static string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTczOTcwMjc2MSwiYXVkIjoiQ3VycmVuY3lFeGNoYW5nZXIifQ.b9yaUdrJVWUC4QZXHkN4QNggNAoN_P5yPNUm7CXsX4s";
        public static string _token;

        #region API path

        public string api = "/api/v1/login";

        public string converterBase = "/api/v1/convert";
        public string converterWithLimit = "/api/v1/convert/limit";

        public string historyRange = "/api/v1/history/range";
        public string historyRangeBase = "/api/v1/history/rangewithbase";
        public string historyRangeWithLimit = "/api/v1/history/rantewithlimit";

        #endregion API path
    }
}