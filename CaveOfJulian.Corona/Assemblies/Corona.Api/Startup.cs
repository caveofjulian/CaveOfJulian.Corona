using System;
using System.Net.Http;
using Corona.Api.Controllers;
using Corona.Persistence;
using Corona.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Roni.Corona.Persistence;

namespace Corona.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<HttpClient>(new HttpClient()
            {
                BaseAddress = new Uri("https://api.thevirustracker.com/free-api")
            });

            services.AddTransient<ICoronaService, CoronaService>();
            services.AddSingleton<ILogger<CoronaController>, Logger<CoronaController>>();
            services.ConfigurePersistenceServices(Configuration.GetConnectionString("RoniDb"));
            services.ConfigureServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
