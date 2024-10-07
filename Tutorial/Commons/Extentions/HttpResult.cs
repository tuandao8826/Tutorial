using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;
using Tutorial.Repositories;
using Tutorial.Services;
using Tutorial.Services.Implementations;

namespace Tutorial.Commons.Extentions
{
    public class HttpResult : HttpResponseMessage
    {
        public TimeSpan Duration { get; internal set; }

        public event Action<Exception>? OnError;

        private readonly Action<Exception> defaultLogError = (ex) =>
        {
            Log.Error("---> An error occurred: {error}", ex);
        };
        private readonly IHttpResultRepository _httpResultRepository;

        public HttpResult(IHttpResultRepository httpResultRepository)
        {
            this._httpResultRepository = httpResultRepository;
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
                return await _httpResultRepository.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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
                return await _httpResultRepository.ReadFromJsonAsync<TResponse>(options, cancellationToken).ConfigureAwait(false);
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
                return await _httpResultRepository.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
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
                return await _httpResultRepository.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
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
