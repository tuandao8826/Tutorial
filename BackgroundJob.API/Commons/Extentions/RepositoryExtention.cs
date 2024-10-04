using BackgroundJob.API.Repositories;
using BackgroundJob.API.Services;

namespace BackgroundJob.API.Commons.Extentions
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
