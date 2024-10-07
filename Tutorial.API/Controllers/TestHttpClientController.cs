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

        public TestHttpClientController(IHttpClientSender httpClientSender)
        {
            this._httpClientSender = httpClientSender;
        }

        [HttpGet("GetDataAsync")]
        public async Task<IActionResult> Get()
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
