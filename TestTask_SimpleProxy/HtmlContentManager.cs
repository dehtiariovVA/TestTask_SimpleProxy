using HtmlAgilityPack;
using TestTask_SimpleProxy.Helpers;

namespace TestTask_SimpleProxy
{
    public class HtmlContentManager : IHtmlContentManager
    {
        private const string LinkToStaticContentAttribute = "src";

        private const string ScriptNode = "script";
        private const string StyleNode = "style";

        private readonly HtmlDocument doc;
        private readonly IUriProvider uriProvider;

        public HtmlContentManager(IUriProvider uriProvider)
        {
            doc = new HtmlDocument();
            this.uriProvider = uriProvider;
        }

        public void ModifyText(string text, Func<string, string> func)
        {
            doc.LoadHtml(text);

            foreach (var textNode in doc.DocumentNode.DescendantsAndSelf()
                .Where(n => n.NodeType == HtmlNodeType.Text && n.ParentNode.Name != ScriptNode && n.ParentNode.Name != StyleNode))
            {
                textNode.InnerHtml = func(textNode.InnerHtml);
            }
        }

        public void SetBasePathForStaticContent(string baseUri)
        {
            foreach (var node in doc.DocumentNode.Descendants())
            {
                var attributeValue = node.GetAttributeValue(LinkToStaticContentAttribute, null);

                if (attributeValue != null)
                {
                    node.SetAttributeValue(LinkToStaticContentAttribute, uriProvider.GetUri(baseUri, attributeValue).ToString());
                }
            }
        }

        public override string ToString()
        {
            return doc.DocumentNode.OuterHtml;
        }
    }
}
