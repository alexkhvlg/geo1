using Newtonsoft.Json;

namespace GeoWrapper.Services
{
    public class ConnectionParameterEntry
    {
        [JsonProperty("@key")]
        public string Key { get; set; }

        [JsonProperty("$")]
        public string Value { get; set; }

        public static ConnectionParameterEntry Create(string key, string value)
        {
            return new ConnectionParameterEntry
            {
                Key = key,
                Value = value
            };
        }
    }
}