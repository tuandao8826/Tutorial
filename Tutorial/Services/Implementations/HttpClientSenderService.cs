namespace Tutorial.Services.Implementations
{
    public class HttpClientSenderService : IHttpClientSenderService
    {
        private readonly IMapper mapper;

        public HttpClientSenderService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        private static readonly SocketsHttpHandler Handler = new()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        };

        private static readonly HttpClient DefaultHttpClient = new(Handler);
        private HttpRequestMessage request = new();
        private bool isDefaultlog = true;
        private HttpClient? httpClient;

        public IHttpClientSenderService UseClient(HttpClient httpClient)
        {
            request = new();
            isDefaultlog = false;
            this.httpClient = httpClient;
            return this;
        }

        public IHttpClientSenderService UseMethod(HttpMethod method)
        {
            request.Method = method;
            return this;
        }

        public IHttpClientSenderService WithUri(string uri)
        {
            request.RequestUri = new Uri(uri);
            return this;
        }

        public IHttpClientSenderService WithUri(Uri uri)
        {
            request.RequestUri = uri;
            return this;
        }

        public IHttpClientSenderService WithContent(HttpContent content)
        {
            request.Content = content;
            return this;
        }

        public IHttpClientSenderService WithHeaders(object headers, bool replaceUnderscoreWithHyphen = true)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            // underscore replacement only applies when object properties are parsed to kv pairs
            replaceUnderscoreWithHyphen = replaceUnderscoreWithHyphen && headers is not string && headers is not IEnumerable;

            foreach (var res in ParseKeyValuePairs(headers))
            {
                string replaceKey = replaceUnderscoreWithHyphen ? res.Key.Replace("_", "-", StringComparison.Ordinal) : res.Key;
                request.Headers.Add(replaceKey, res.Value?.ToString());
            }

            return this;
        }

        public async Task<HttpResult> SendAsync(CancellationToken cancellationToken = default)
        {
            TimeSpan duration = TimeSpan.Zero;
            try
            {
                HttpClient client = httpClient ?? DefaultHttpClient;
                if (isDefaultlog)
                {
                    Log.Information("---> Request Info: \n{request}\n---> End", request.ToString());
                }

                DateTime begin = DateTime.UtcNow;
                HttpResponseMessage response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                DateTime end = DateTime.UtcNow;
                HttpResult httpResult = mapper.Map<HttpResult>(response);
                httpResult.Duration = end - begin;
                if (isDefaultlog)
                {
                    Log.Information("---> Response Info: \n{reponse}\n---> End", httpResult.ToString());
                }

                return httpResult;
            }
            catch (Exception ex)
            {
                return new(duration, ex, request);
            }
            finally
            {
                request = new();
            }
        }

        /// <summary>
        /// Returns a key-value-pairs representation of the object.
        /// For strings, URL query string format assumed and pairs are parsed from that.
        /// For objects that already implement IEnumerable&lt;KeyValuePair&gt;, the object itself is simply returned.
        /// For all other objects, all publicly readable properties are extracted and returned as pairs.
        /// </summary>
        /// <param name="obj">The object to parse into key-value pairs</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null" />.</exception>
        private static IEnumerable<(string Key, object? Value)> ParseKeyValuePairs(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is IEnumerable e)
            {
                return
                obj is string s ? StringToKeyValue(s) :
                (IEnumerable<(string, object? Value)>)CollectionToKeyPair(e);
            }
            else
            {
                return
                obj is string s ? StringToKeyValue(s) :
                ObjectToKeyValue(obj);
            }
        }

        private static IEnumerable<(string Key, object? Value)> StringToKeyValue(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return Enumerable.Empty<(string, object?)>();
            }

            return
                from p in s.Split('&')
                let pair = SplitOnFirstOccurence(p, "=")
                let name = pair[0]
                let value = pair.Length == 1 ? null : pair[1]
                select (name, (object)value);
        }

        /// <summary>
        /// Splits at the first occurrence of the given separator.
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <param name="separator">The separator to split on.</param>
        /// <returns>Array of at most 2 strings. (1 if separator is not found.)</returns>
        private static string[] SplitOnFirstOccurence(string s, string separator)
        {
            // Needed because full PCL profile doesn't support Split(char[], int) (#119)
            if (string.IsNullOrEmpty(s))
            {
                return new[] { s };
            }

            var i = s.IndexOf(separator);
            return i == -1 ?
                new[] { s } :
                new[] { s[..i], s[(i + separator.Length)..] };
        }

        private static IEnumerable<(string Name, object? Value)> ObjectToKeyValue(object obj) =>
            from prop in obj.GetType().GetProperties()
            let getter = prop.GetGetMethod(false)
            where getter != null
            let val = getter.Invoke(obj, null)
            select (prop.Name, GetDeclaredTypeValue(val, prop.PropertyType));

        private static object? GetDeclaredTypeValue(object value, Type declaredType)
        {
            if (value == null || value.GetType() == declaredType)
            {
                return value;
            }

            declaredType = Nullable.GetUnderlyingType(declaredType) ?? declaredType;

            if (value is IEnumerable col
                && declaredType.IsGenericType
                && declaredType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                && !col.GetType().GetInterfaces().Contains(declaredType)
                && declaredType.IsInstanceOfType(col))
            {
                var elementType = declaredType.GetGenericArguments()[0];
                return col.Cast<object>().Select(element => Convert.ChangeType(element, elementType));
            }

            return value;
        }

        private static IEnumerable<(string Key, object? Value)> CollectionToKeyPair(IEnumerable col)
        {
            bool TryGetProp(object obj, string name, out object? value)
            {
                var prop = obj.GetType().GetProperty(name);
                var field = obj.GetType().GetField(name);

                if (prop != null)
                {
                    value = prop.GetValue(obj, null);
                    return true;
                }

                if (field != null)
                {
                    value = field.GetValue(obj);
                    return true;
                }

                value = null;
                return false;
            }

            bool IsTuple2(object item, out object? name, out object? val)
            {
                name = null;
                val = null;
                return
                    OrdinalContains(item.GetType().Name, "Tuple") &&
                    TryGetProp(item, "Item1", out name) &&
                    TryGetProp(item, "Item2", out val) &&
                    !TryGetProp(item, "Item3", out _);
            }

            bool LooksLikeKV(object item, out object? name, out object? val)
            {
                name = null;
                val = null;
                return
                    (TryGetProp(item, "Key", out name) || TryGetProp(item, "key", out name) || TryGetProp(item, "Name", out name) || TryGetProp(item, "name", out name)) &&
                    (TryGetProp(item, "Value", out val) || TryGetProp(item, "value", out val));
            }

            foreach (var item in col)
            {
                if (item == null)
                {
                    continue;
                }

                if (!IsTuple2(item, out var name, out var val) && !LooksLikeKV(item, out name, out val))
                {
                    yield return (ToInvariantString(name) ?? throw new ArgumentNullException(nameof(col)), null);
                }
                else if (name != null)
                {
                    yield return (ToInvariantString(name) ?? throw new ArgumentNullException(nameof(col)), val);
                }
            }
        }

        private static bool OrdinalContains(string s, string value, bool ignoreCase = false) =>
                s?.IndexOf(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0;

        /// <summary>
        /// Returns a string that represents the current object, using CultureInfo.InvariantCulture where possible.
        /// Dates are represented in IS0 8601.
        /// </summary>
        private static string? ToInvariantString(object? obj)
        {
            if (obj == null)
            {
                return null;
            }
            else
            {
                if (obj is DateTime dt)
                {
                    return dt.ToString("o", CultureInfo.InvariantCulture);
                }
                else if (obj is DateTimeOffset dto)
                {
                    return dto.ToString("o", CultureInfo.InvariantCulture);
                }
                else if (obj is IConvertible c)
                {
                    return c.ToString(CultureInfo.InvariantCulture);
                }
                else if (obj is IFormattable f)
                {
                    return f.ToString(null, CultureInfo.InvariantCulture);
                }
                else
                {
                    return obj.ToString();
                }
            }
        }
    }
}
