using Tutorial.Repositories;
using Tutorial.Services;

namespace Tutorial.Commons.Extentions
{
    public static class RepositoryExtention
    {
        public static void AddRepositoryExtention(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
        }
    }
}
