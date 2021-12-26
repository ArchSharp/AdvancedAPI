using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Utilize this to register other application services that do not utilize a scoped lifetime
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // services.AddScoped<I, UserActivityRepository>();
        }
    }
}