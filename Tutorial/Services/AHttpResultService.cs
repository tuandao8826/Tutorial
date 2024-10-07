using Serilog;
using System.Text.Json;

namespace Tutorial.Services
{
    public abstract class AHttpResultService : HttpResponseMessage
    {
        public TimeSpan Duration { get; internal set; }

        public event Action<Exception>? OnError;

        public readonly Action<Exception> defaultLogError = (ex) =>
        {
            Log.Error("---> An error occurred: {error}", ex);
        };

        public abstract Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default);
        public abstract Task<TResponse?> ReadFromJsonAsync<TResponse>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default);
        public abstract Task<Stream?> ReadAsStreamAsync(CancellationToken cancellationToken = default);
        public abstract Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default);
    }
}
