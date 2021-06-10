using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeoWrapper.Models
{
    public class FeatureTypeDetailInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("nativeName")]
        public string NativeName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("srs")]
        public string Srs { get; set; }

        [JsonProperty("nativeBoundingBox")]
        public BoundingBox NativeBoundingBox { get; set; }

        [JsonProperty("latLonBoundingBox")]
        public BoundingBox LatLonBoundingBox { get; set; }

        [JsonProperty("overridingServiceSRS")]
        public bool OverridingServiceSrs { get; set; }

        [JsonProperty("skipNumberMatched")]
        public bool SkipNumberMatched { get; set; }

        [JsonProperty("circularArcPresent")]
        public bool CircularArcPresent { get; set; }

        [JsonProperty("keywords")]
        public KeywordsContainer KeywordsContainer { get; set; }

        [JsonProperty("attributes")]
        public FeatureTypeAttributesContainer FeatureTypeAttributes { get; set; }

        [JsonProperty("projectionPolicy")]
        public string ProjectionPolicy { get; set; }

        [JsonProperty("advertised")]
        public bool Advertised { get; set; }
    }

    public class FeatureTypeAttributesContainer
    {

        [JsonProperty("attribute")]
        public ICollection<FeatureTypeAttribute> FeatureTypeAttributes { get; set; }
    }

    public class FeatureTypeAttribute
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("binding")]
        public string Binding { get; set; } // java.lang.Integer, java.lang.String, java.math.BigDecimal

        [JsonProperty("minOccurs")]
        public int MinOccurs { get; set; }

        [JsonProperty("maxOccurs")]
        public int MaxOccurs { get; set; }

        [JsonProperty("nillable")]
        public bool Nullable { get; set; }

        public static FeatureTypeAttribute Create(string name, string binding, int minOccurs = 0, int maxOccurs = 1, bool nullable = true)
        {
            return new FeatureTypeAttribute
            {
                Name = name,
                Binding = binding,
                MinOccurs = minOccurs,
                MaxOccurs = maxOccurs,
                Nullable = nullable
            };
        }
    }

    public class BoundingBox
    {
        [JsonProperty("minx")]
        public double MinX { get; set; }

        [JsonProperty("miny")]
        public double MinY { get; set; }

        [JsonProperty("maxx")]
        public double MaxX { get; set; }

        [JsonProperty("maxy")]
        public double MaxY { get; set; }

        [JsonProperty("crs")]
        public string Crs { get; set; }
    }

    public class KeywordsContainer
    {
        [JsonProperty("string")]
        public string[] Strings { get; set; }
    }
}