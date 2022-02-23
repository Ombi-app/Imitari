using Imitari.Apis;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Imitari.Func.Startup))]
namespace Imitari.Func
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddRefitClient<IFanartTvApi>()
                .ConfigureHttpClient(c =>
                {
                    //c.DefaultRequestHeaders.Add("api-key", Environment.GetEnvironmentVariable("FanartApiKey"));
                    c.DefaultRequestHeaders.Add("api-key", "4b6d983efa54d8f45c68432521335f15");
                    c.BaseAddress = new Uri("https://webservice.fanart.tv/v3");
                });

            builder.Services
                .AddRefitClient<ITheMovieDbApi>()
                .ConfigureHttpClient(c => { c.BaseAddress = new Uri("http://api.themoviedb.org/3"); });

        }
    }
}
