using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial.Models;
using Tutorial.Services;

namespace Tutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _productService.Add(product);
            return Ok("Success");
        }
    }
}
