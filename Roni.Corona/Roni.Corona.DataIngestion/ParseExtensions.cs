namespace Roni.Corona.DataIngestion
{
    public static class ParseExtensions
    {
        public static int ParseToInt(this string x)
        {
            int.TryParse(x, out int result);
            return result;
        }
    }
}