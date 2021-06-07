using System.Collections.Generic;
using GeoWrapper.Services;
using Newtonsoft.Json;

namespace GeoWrapper.Models
{
    public class DataStoreDetailInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("workspace")]
        public Workspace Workspace { get; set; }

        [JsonProperty("connectionParameters")]
        public ConnectionParameters ConnectionParameters { get; set; }

        [JsonProperty("_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("featureTypes")]
        public string FeatureTypesUrl { get; set; }
    }
}
