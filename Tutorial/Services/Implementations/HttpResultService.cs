using System.Text.Json;

namespace Tutorial.Services.Implementations
{
    public class HttpResultService : IHttpResultService
    {
        private readonly HttpContent _content;

        public HttpResultService(HttpContent content)
        {
            _content = content;
        }

        public async Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default)
        {
            return await _content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse?> ReadFromJsonAsync<TResponse>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            return await _content.ReadFromJsonAsync<TResponse>(options, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Stream?> ReadAsStreamAsync(CancellationToken cancellationToken = default)
        {
            return await _content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default)
        {
            return await _content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }
    }

}
