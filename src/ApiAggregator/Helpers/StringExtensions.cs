namespace ApiAggregator.Net.Helpers
{
    public static class StringExtensions
    {
        public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
    }
}