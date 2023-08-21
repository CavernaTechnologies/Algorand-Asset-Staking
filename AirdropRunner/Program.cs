using Algorand.V2.Algod;
using Algorand.V2.Indexer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace AirdropRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            await host.Services.GetService<App>().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging(configure => configure.AddConsole());

                    services.AddHttpClient<IDefaultApi, DefaultApi>(client =>
                    {
                        client.BaseAddress = new Uri(context.Configuration["Endpoints:Algod"]);
                        client.Timeout = Timeout.InfiniteTimeSpan;
                    });

                    services.AddHttpClient<ILookupApi, LookupApi>(client =>
                    {
                        client.BaseAddress = new Uri(context.Configuration["Endpoints:Indexer"]);
                        client.Timeout = Timeout.InfiniteTimeSpan;
                    });

                    services.AddHttpClient<ISearchApi, SearchApi>(client =>
                    {
                        client.BaseAddress = new Uri(context.Configuration["Endpoints:Indexer"]);
                        client.Timeout = Timeout.InfiniteTimeSpan;
                    });

                    services.AddTransient<IAlgodUtils, AlgodUtils>();
                    services.AddTransient<IIndexerUtils, IndexerUtils>();
                    services.AddTransient<App>();
                });
    }
}
