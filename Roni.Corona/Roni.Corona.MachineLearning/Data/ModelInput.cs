﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Roni.Corona.MachineLearning.Data
{
    public class ModelInput
    {
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public float Confirmed { get; set; }
        public float Death { get; set; }
        public float Recovered { get; set; }
    }
}
