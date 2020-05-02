using System;
using System.ComponentModel.DataAnnotations;

namespace Roni.Corona.Shared
{
    public class Cases
    {
        [Key]
        public int Id { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Confirmed { get; set; }
        public int Death { get; set; }
        public int Recovered { get; set; }
    }
}
