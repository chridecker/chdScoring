using System.Text.Json;

namespace chdScoring.App.UI.Constants
{
    public class SerializationConstants
    {
        public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}
