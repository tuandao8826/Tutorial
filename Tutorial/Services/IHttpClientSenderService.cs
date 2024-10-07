using Tutorial.Commons.Extentions;

namespace Tutorial.Services
{
    public interface IHttpClientSenderService
    {
        /// <summary>
        /// Use to set client instance if not using default client
        /// </summary>
        IHttpClientSenderService UseClient(HttpClient httpClient);

        /// <summary>
        /// set http method (default is get)
        /// </summary>
        IHttpClientSenderService UseMethod(HttpMethod method);

        /// <summary>
        /// set request uri (default is empty)
        /// </summary>
        IHttpClientSenderService WithUri(string uri);

        /// <summary>
        /// set request uri (default is empty)
        /// </summary>
        IHttpClientSenderService WithUri(Uri uri);

        /// <summary>
        /// Header is object or keyvaluepair
        /// </summary>
        /// <param name="headers">Names/values of HTTP headers to set. Typically an anonymous object or IDictionary.</param>
        /// <param name="replaceUnderscoreWithHyphen">If true, underscores in property names will be replaced by hyphens. Default is true.</param>
        /// <exception cref="ArgumentNullException"><paramref name="headers"/> is <c>null</c>.</exception>
        IHttpClientSenderService WithHeaders(object headers, bool replaceUnderscoreWithHyphen = true);

        /// <summary>
        /// set request content (default is empty)
        /// </summary>
        IHttpClientSenderService WithContent(HttpContent content);

        Task<HttpResult> SendAsync(CancellationToken cancellationToken = default);
    }
}
