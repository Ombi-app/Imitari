using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imitari.Apis;
using Imitari.Services;
using Refit;

namespace Imitari
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

            services
                .AddRefitClient<IFanartTvApi>()
                .ConfigureHttpClient(c =>
                {
                    //c.DefaultRequestHeaders.Add("api-key", Environment.GetEnvironmentVariable("FanartApiKey"));
                    c.DefaultRequestHeaders.Add("api-key", "4b6d983efa54d8f45c68432521335f15");
                    c.BaseAddress = new Uri("https://webservice.fanart.tv/v3");
                });

            services
                .AddRefitClient<ITheMovieDbApi>()
                .ConfigureHttpClient(c => { c.BaseAddress = new Uri("http://api.themoviedb.org/3/"); });

            services.AddLazyCache();
            services.AddSingleton<ICacheService, CacheService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Imitari", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Imitari v1"));
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
