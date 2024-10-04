using Microsoft.EntityFrameworkCore;

namespace BackgroundJob.API.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ProductDbContext()
        {
        }

        #region DBSet
        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Product> Products { get; set; }
        #endregion DBSet
    }
}
