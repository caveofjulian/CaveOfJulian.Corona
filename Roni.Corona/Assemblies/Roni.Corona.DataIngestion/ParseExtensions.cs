namespace Roni.Corona.DataIngestion
{
    public static class ParseExtensions
    {
        public static int ParseToInt(this string x)
        {
            var isParsed = int.TryParse(x, out var result);

            return isParsed ? result : 0;
        }
    }
}
