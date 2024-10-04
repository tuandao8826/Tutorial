using BackgroundJob.API.Models;

namespace BackgroundJob.API.Repositories
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<Product?> ProductGetById(int id);
    }
}
