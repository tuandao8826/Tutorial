using System.Text.Json;

namespace HttpClientTutorial.API.HttpClients
{
    public interface IHttpResult
    {
        Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default);
        Task<TResponse?> ReadFromJsonAsync<TResponse>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default);
        Task<Stream?> ReadAsStreamAsync(CancellationToken cancellationToken = default);
        Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default);
    }
}
