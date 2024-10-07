using Tutorial.Models;
using Tutorial.Repositories;

namespace Tutorial.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public void Add(Product product)
        {
            _productRepository.Add(product);
        }

        public void Delete(Product product)
        {
            _productRepository.Delete(product);
        }

        public async Task<Product?> Get(int id)
        {
            return await _productRepository.ProductGetById(id);
        }

        public void Update(Product product)
        {
            _productRepository.Delete(product);
        }
    }
}
