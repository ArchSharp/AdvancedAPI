using System.Reflection;
using API.Extensions;
using Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShareLoanApp.API.Middlewares;
using ShareLoanApp.Application.Helpers;
using ShareLoanApp.Application.Services.Implementations;
using ShareLoanApp.Application.Services.Interfaces;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", reloadOnChange: true,
                    optional: true)
                .AddUserSecrets(Assembly.GetAssembly(typeof(Startup)))
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIisIntegration();
            services.ConfigureLoggerService();
            services.ConfigureIdentity();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureHangfire(Configuration);
            services.AddAuthentication();
            services.ConfigureJwt(Configuration);
            services.AddHttpContextAccessor();
            services.AddControllers()
                .AddXmlDataContractSerializerFormatters();
            services.ConfigureSwagger();
            services.ConfigureApiVersioning(Configuration);
            services.ConfigureMvc();
            services.ConfigureGlobalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", 
                        description.GroupName.ToUpperInvariant());
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseErrorHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            WebHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }
    }
}
