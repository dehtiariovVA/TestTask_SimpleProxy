namespace TestTask_SimpleProxy.Helpers
{
    public interface IUriProvider
    {
        Uri GetUri(string baseUri, string link);
    }
}
