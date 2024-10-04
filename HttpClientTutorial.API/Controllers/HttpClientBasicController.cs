using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientTutorial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpClientBasicController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public HttpClientBasicController(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        [HttpGet("GetDataAsync")]
        public async Task<IActionResult> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(response.Headers);
            }

            return BadRequest();
        }
    }
}
