using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GeoWrapper.Models
{
    public class WorkspaceDetailInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isolated")]
        public bool Isolated { get; set; }

        [JsonProperty("dateCreated"), JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("dataStores")]
        public string DataStores { get; set; }

        [JsonProperty("coverageStores")]
        public string CoverageStores { get; set; }

        [JsonProperty("wmsStores")]
        public string WmsStores { get; set; }

        [JsonProperty("wmtsStores")]
        public string WmtsStores { get; set; }
    }

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff \\U\\T\\C";
            DateTimeStyles = DateTimeStyles.AssumeUniversal;
        }
    }
}