using Newtonsoft.Json;

namespace GeoWrapper.Models
{
    public class Workspace
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}