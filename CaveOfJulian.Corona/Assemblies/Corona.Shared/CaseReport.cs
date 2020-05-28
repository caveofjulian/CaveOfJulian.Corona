using System;
using Newtonsoft.Json;

namespace Corona.Shared
{
    public class CaseReport
    {
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastUpdated { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Confirmed { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Death { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Recovered { get; set; }
    }
}
