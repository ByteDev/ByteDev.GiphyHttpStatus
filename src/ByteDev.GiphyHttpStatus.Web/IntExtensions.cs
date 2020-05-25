namespace ByteDev.GiphyHttpStatus.Web
{
    public static class IntExtensions
    {
        public static bool IsBetween(this int source, int min, int max)
        {
            return source >= min && source <= max;
        }
    }
}