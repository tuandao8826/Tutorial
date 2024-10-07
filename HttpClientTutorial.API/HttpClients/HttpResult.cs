using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using System.Text;

namespace HttpClientTutorial.API.HttpClients
{
    public class HttpResult : HttpResponseMessage, IHttpResult
    {
        public event Action<Exception>? OnError;

        private readonly Action<Exception> defaultLogError = (ex) =>
        {
            Log.Error("---> An error occurred: {error}", ex);
        };
        public TimeSpan Duration { get; internal set; }


        public HttpResult()
        {
        }

        public HttpResult(TimeSpan duration, Exception requestException, HttpRequestMessage request)
        {
            Duration = duration;
            RequestMessage = request;
            StatusCode = HttpStatusCode.InternalServerError;
            LogException(requestException);
        }

        public async Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogException(ex);
                StatusCode = HttpStatusCode.InternalServerError;
                return default;
            }
        }

        public async Task<TResponse?> ReadFromJsonAsync<TResponse>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await Content.ReadFromJsonAsync<TResponse>(options, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogException(ex);
                StatusCode = HttpStatusCode.InternalServerError;
                return default;
            }
        }

        public async Task<Stream?> ReadAsStreamAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogException(ex);
                StatusCode = HttpStatusCode.InternalServerError;
                return default;
            }
        }

        public async Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogException(ex);
                StatusCode = HttpStatusCode.InternalServerError;
                return Array.Empty<byte>();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());
            sb.AppendLine("', Duration: ");
            sb.Append(Duration);
            return sb.ToString();
        }

        private void LogException(Exception ex)
        {
            if (OnError != null)
            {
                OnError?.Invoke(ex);
            }
            else
            {
                defaultLogError.Invoke(ex);
            }
        }
    }
}
