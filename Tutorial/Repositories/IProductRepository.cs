using Tutorial.Models;

namespace Tutorial.Repositories
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<Product?> ProductGetById(int id);
    }
}
