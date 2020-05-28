using System;

namespace Corona.DataIngestion
{
    internal class MLCase
    {
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public int? Confirmed { get; set; }
        public int? Death { get; set; }
        public int? Recovered { get; set; }
    }
}