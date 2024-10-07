using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using Tutorial.Infrastructure.Facades.Common.HttpClients;
using Tutorial.Infrastructure.Facades.Common.HttpClients.Interfaces;

namespace Tutorial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHttpClientController : ControllerBase
    {
        private readonly IHttpClientSender _httpClientSender;
        private readonly HttpClient _httpClient;

        public TestHttpClientController(IHttpClientSender httpClientSender, HttpClient httpClient)
        {
            this._httpClientSender = httpClientSender;
            this._httpClient = httpClient;
        }

        [HttpGet("GetDataUseClientAsync")]
        public async Task<IActionResult> GetDataUseClientAsync()
        {
            var urlString = "https://jsonplaceholder.typicode.com/posts";

            var result = await _httpClientSender
                .UseClient(_httpClient)
                .WithUri(urlString)
                .UseMethod(HttpMethod.Get)
                .SendAsync();

            return Ok(result.ReadAsStringAsync().Result);
        }

        [HttpGet("GetDataDefaultClientAsync")]
        public async Task<IActionResult> GetDataDefaultClientAsync()
        {
            var urlString = "https://jsonplaceholder.typicode.com/posts";

            var result = await _httpClientSender
                .WithUri(urlString)
                .UseMethod(HttpMethod.Get)
                .SendAsync();

            return Ok(result.ReadAsStringAsync().Result);
        }

        [HttpPost("PostDataAsync")]
        public async Task<IActionResult> PostDataAsync()
        {
            var urlString = "https://jsonplaceholder.typicode.com/posts";

            var post = new
            {
                UserId = 1,
                Title = "ABC",
                Body = "XYZ",
            };

            var result = await _httpClientSender
                .WithUri(urlString)
                .UseMethod(HttpMethod.Post)
                .WithContent(HttpClientExtensions.ToStringContent(post))
                .SendAsync();

            return Ok(result.ReadAsStringAsync().Result);
        }
    }
}
