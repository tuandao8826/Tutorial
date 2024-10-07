using AutoMapper;
using Serilog;
using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using Tutorial.Infrastructure.Facades.Common.Helpers;
using Tutorial.Infrastructure.Facades.Common.HttpClients.Interfaces;

namespace Tutorial.Infrastructure.Facades.Common.HttpClients;

public class HttpClientSender : IHttpClientSender
{
    private readonly IMapper _mapper;
    private HttpRequestMessage _request = new HttpRequestMessage();
    private bool _isDefaultlog = true;
    private HttpClient? _httpClient;

    private static readonly SocketsHttpHandler _handler = new()
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
    };

    // - Tái sử dụng HttpClient để tránh hết Socket: sử dụng Static để new instance một lần
    // duy nhất khi lớp HttpClientSender được sử dụng lần đầu tiền
    private static readonly HttpClient _defaultHttpClient = new(_handler);

    public HttpClientSender(IMapper mapper)
    {
        this._mapper = mapper;
    }

    public IHttpClientSender UseClient(HttpClient httpClient)
    {
        _request = new();
        _isDefaultlog = false;
        this._httpClient = httpClient;
        return this;
    }

    public IHttpClientSender UseMethod(HttpMethod method)
    {
        _request.Method = method;
        return this;
    }

    public IHttpClientSender WithUri(string uri)
    {
        _request.RequestUri = new Uri(uri);
        return this;
    }

    public IHttpClientSender WithUri(Uri uri)
    {
        _request.RequestUri = uri;
        return this;
    }

    public IHttpClientSender WithContent(HttpContent content)
    {
        _request.Content = content;
        return this;
    }

    public IHttpClientSender WithHeaders(object headers, bool replaceUnderscoreWithHyphen = true)
    {
        if (headers == null)
        {
            throw new ArgumentNullException(nameof(headers));
        }

        // underscore replacement only applies when object properties are parsed to kv pairs
        replaceUnderscoreWithHyphen = replaceUnderscoreWithHyphen && headers is not string && headers is not IEnumerable;

        foreach (var res in KeyValueHelper.ParseKeyValuePairs(headers))
        {
            string replaceKey = replaceUnderscoreWithHyphen ? res.Key.Replace("_", "-", StringComparison.Ordinal) : res.Key;
            _request.Headers.Add(replaceKey, res.Value?.ToString());
        }

        return this;
    }

    public async Task<HttpResult> SendAsync(CancellationToken cancellationToken = default)
    {
        TimeSpan duration = TimeSpan.Zero;

        try
        {
            HttpClient client = _httpClient ?? _defaultHttpClient;

            if (_isDefaultlog)
            {
                Log.Information($"--->_request Info: \n{_request}\n---> End", _request.ToString());
            }

            DateTime begin = DateTime.UtcNow;
            // ConfigureAwait(false) được sử dụng để không cần quay lại luồng ban đầu sau khi tác vụ hoàn thành, giúp tăng hiệu năng.
            HttpResponseMessage response = await client.SendAsync(_request, cancellationToken).ConfigureAwait(false);
            DateTime end = DateTime.UtcNow;
            HttpResult httpResult = _mapper.Map<HttpResult>(response);
            httpResult.Duration = end - begin;

            if (_isDefaultlog)
            {
                Log.Information($"---> Response Info: \n{httpResult}\n---> End", httpResult.ToString());
            }

            return httpResult;
        }
        catch (Exception ex)
        {
            return new(duration, ex, _request);
        }
        // Đảm bảo _request thành một đối tượng HttpRequestMessage để chuẩn bị cho yêu cầu tiếp theo
        finally
        {
            _request = new();
        }
    }
}