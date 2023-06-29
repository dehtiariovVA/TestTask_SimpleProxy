using System.Net.Mime;
using System.Text.RegularExpressions;
using TestTask_SimpleProxy.Extensions;
using TestTask_SimpleProxy.Helpers;

namespace TestTask_SimpleProxy.Middlewares
{
    public class ProxyMiddleware
    {
        private const string SiteHost = "https://learn.microsoft.com/";

        private const string SixCharacterWordRegex = @"\b\w{6}\b";
        private const string TmSign = "™";

        private readonly RequestDelegate nextMiddleware;

        private readonly IHttpClientFactory httpClientFactory;

        public ProxyMiddleware(RequestDelegate nextMiddleware, IHttpClientFactory httpClientFactory)
        {
            this.nextMiddleware = nextMiddleware;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context, IHtmlContentManager contentManager, IUriProvider uriProvider)
        {
            var httpClient = httpClientFactory.CreateClient();

            Uri uri = uriProvider.GetUri(SiteHost, context.Request.Path);

            if (uri != null)
            {
                HttpRequestMessage message = CreateTargetMessage(context, uri);

                var responseMessage = await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);

                context.Response.StatusCode = (int)responseMessage.StatusCode;

                context.CopyResponseHeaders(responseMessage);

                await ProcessResponseContent(contentManager, context, responseMessage);

                return;
            }

            await nextMiddleware(context);
        }

        private async Task ProcessResponseContent(
            IHtmlContentManager contentManager, 
            HttpContext context, 
            HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            
            if (context.Response.ContentType == MediaTypeNames.Text.Html)
            {
                contentManager.ModifyText(
                    content,
                    nodeText => Regex.Replace(nodeText, SixCharacterWordRegex, m => m.Value + TmSign));

                contentManager.SetBasePathForStaticContent(SiteHost);

                content = contentManager.ToString();
            }

            await context.Response.WriteAsync(content);
        }

        private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
        {
            var requestMessage = new HttpRequestMessage();

            if (context.Request.ContentLength > 0)
            {
                requestMessage.Content = new StreamContent(context.Request.Body);
            }

            foreach (var header in context.Request.Headers)
            {
                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            requestMessage.RequestUri = targetUri;
            requestMessage.Headers.Host = targetUri.Host;
            requestMessage.Method = new HttpMethod(context.Request.Method);

            return requestMessage;
        }
    }
}
