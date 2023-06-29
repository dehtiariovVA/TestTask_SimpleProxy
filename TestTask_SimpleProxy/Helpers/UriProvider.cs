namespace TestTask_SimpleProxy.Helpers
{
    public class UriProvider : IUriProvider
    {
        public Uri GetUri(string baseUri, string link)
        {
            bool IsAbsoluteUri = Uri.TryCreate(link, UriKind.Absolute, out var uri);

            if (!IsAbsoluteUri)
            {
                uri = new Uri(new Uri(baseUri), link);
            }

            return uri;
        }
    }
}
