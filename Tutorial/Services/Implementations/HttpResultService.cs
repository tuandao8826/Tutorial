using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;
using Tutorial.Repositories;

namespace Tutorial.Services.Implementations
{
    public class HttpResultService : AHttpResultService
    {
        private readonly IHttpResultRepository _httpResultRepository;

        public HttpResultService(IHttpResultRepository httpResultRepository)
        {
            this._httpResultRepository = httpResultRepository;
        }

        public HttpResultService(TimeSpan duration, Exception requestException, HttpRequestMessage request)
        {
            Duration = duration;
            RequestMessage = request;
            StatusCode = HttpStatusCode.InternalServerError;
            LogException(requestException);
        }

        public async override Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default)
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

        public async override Task<TResponse?> ReadFromJsonAsync<TResponse>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
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

        public async override Task<Stream?> ReadAsStreamAsync(CancellationToken cancellationToken = default)
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

        public async override Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default)
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
            //if (OnError != null)
            //{
            //    OnError?.Invoke(ex);
            //}
            //else
            //{
            //    defaultLogError.Invoke(ex);
            //}
        }
    }
}
