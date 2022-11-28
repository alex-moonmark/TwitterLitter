using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterLitter.Server.Classes;
using TwitterLitter.Server.Interfaces;
using TwitterLitter.Server.Services;

namespace TwitterLitter.Tests
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) =>
        hostBuilder
            .ConfigureHostConfiguration(builder => { })
            .ConfigureAppConfiguration((context, builder) => { });

        public void ConfigureServices(IServiceCollection services)
        {


            services.AddSingleton<ICancellationService, CancellationService>();
            services.AddSingleton<ITwitterSampleStreamClient, TwitterClientHandler>();
            services.AddSingleton<ITwitterStatisticsService, TwitterStatisticsService>();
            services.AddSingleton<ITweetProcessingService, TweetProcessingService>();
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }


    }
}
