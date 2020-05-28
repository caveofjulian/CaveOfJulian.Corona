namespace Corona.MachineLearning.Data
{
    public class ModelOutput
    {
        public float[] ForcastedDeath { get; set; }
        public float[] LowerBoundDeath { get; set; }
        public float[] UpperBoundDeath { get; set; }
    }
}
