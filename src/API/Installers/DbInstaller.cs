using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.Data.DbContext;

namespace API.Installers
{
    public class DbInstaller : IInstaller
    {
        public static readonly ILoggerFactory contextLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options
                .UseLoggerFactory(contextLoggerFactory)
                // .UseSqlServer(
                //     configuration["DB_CONNECTION_STRING"]));
                .UseNpgsql(
                    configuration["DB_CONNECTION_STRING"]));
        }
    }
}
