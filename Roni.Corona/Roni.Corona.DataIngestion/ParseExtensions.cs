namespace Roni.Corona.DataIngestion
{
    public static class ParseExtensions
    {
        // No need of using bool to return if parsing succeeded as if fail, returns 0 anyways.
        public static int ParseToInt(this string x)
        {
            int.TryParse(x, out int result);
            return result;
        }
    }
}
