using Tutorial.Repositories;
using Tutorial.Repositories.Implementations;
using Tutorial.Services;
using Tutorial.Services.Implementations;

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
