using BackgroundJob.API.Models;

namespace BackgroundJob.API.Services
{
    public interface IProductService
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<Product?> Get(int id);
    }
}
