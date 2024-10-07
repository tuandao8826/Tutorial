using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Tutorial.Repositories.Implementations
{
    public class HttpResultRepository : IHttpResultRepository
    {
        private readonly HttpContent _httpContent;

        public HttpResultRepository(HttpContent httpContent)
        {
            this._httpContent = httpContent;
        }

        public async Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default)
        {
            return await _httpContent.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Stream?> ReadAsStreamAsync(CancellationToken cancellationToken = default)
        {
            return await _httpContent.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default)
        {
            return await _httpContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse?> ReadFromJsonAsync<TResponse>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            return await _httpContent.ReadFromJsonAsync<TResponse>(options, cancellationToken).ConfigureAwait(false);
        }
    }
}
