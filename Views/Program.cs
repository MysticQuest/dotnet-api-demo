using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Services;
using System;
using System.Threading.Tasks;

namespace Views
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var itemService = host.Services.GetRequiredService<IItemService>();
            var ui = new UserInterface(itemService);
            await ui.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddScoped<IRepository<Item>, Repository<Item>>();
                    services.AddHttpClient();
                    services.AddScoped<IItemService, ItemService>();
                });
    }
}
