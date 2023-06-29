namespace TestTask_SimpleProxy
{
    public interface IHtmlContentManager
    {
        void ModifyText(string text, Func<string, string> func);
        void SetBasePathForStaticContent(string baseUri);
    }
}