using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("GetDataAsync")]
        public async Task<IActionResult> Get()
        {
            var urlString = "https://jsonplaceholder.typicode.com/posts";

            var result = await _httpClientSender
                .UseClient(_httpClient)
                .WithUri(urlString)
                .UseMethod(HttpMethod.Get)
                .SendAsync();

            return Ok(result.ReadAsStringAsync().Result);
        }
    }
}
