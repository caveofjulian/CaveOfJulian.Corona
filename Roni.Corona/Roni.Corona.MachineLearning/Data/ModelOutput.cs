using System;
using System.Collections.Generic;
using System.Text;

namespace Roni.Corona.MachineLearning.Data
{
    public class ModelOutput
    {
        public float[] ForcastedDeath { get; set; }
        public float[] LowerBoundDeath { get; set; }
        public float[] UpperBoundDeath { get; set; }
    }
}
