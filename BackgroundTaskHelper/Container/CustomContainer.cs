using BackgroundTaskHelper.ActionFilters;
using BackgroundTaskHelper.CacheHelper;
using BackgroundTaskHelper.Factory;

namespace BackgroundTaskHelper.Container
{
    public static class CustomContainer
    {
        public static void AddCustomContainer(this IServiceCollection services, IConfiguration configuration)
        {
            IConnenctionFactory connectionFactory = new ConnenctionFactory(configuration.GetConnectionString("connstring")!);
            services.AddSingleton<IConnenctionFactory>(connectionFactory);
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<LogActionFilter>();
        }
    }
}
