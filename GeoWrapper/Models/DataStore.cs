using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeoWrapper.Models
{
    public class DataStore
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}
