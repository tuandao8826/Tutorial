using BackgroundJob.API.Models;

namespace BackgroundJob.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            this._context = context;
        }

        public void Add(Product product)
        {
            var addedEntry = _context.Entry<Product>(product);
            addedEntry.State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.SaveChanges();
        }

        public void Delete(Product product)
        {
            var addedEntry = _context.Entry<Product>(product);
            addedEntry.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
        }

        public async Task<Product?> ProductGetById(int id)
        {
            return _context.Set<Product>().SingleOrDefault(x => x.Id == id);
        }

        public void Update(Product product)
        {
            var addedEntry = _context.Entry<Product>(product);
            addedEntry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
