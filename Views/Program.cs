using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Views
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(new HttpClient())
                .AddSingleton<ApiService>(provider =>
                    new ApiService(provider.GetRequiredService<HttpClient>(), "https://localhost:5001"))
                .BuildServiceProvider();

            var apiService = serviceProvider.GetRequiredService<ApiService>();
            var ui = new UserInterface(apiService);
            ui.RunAsync();
        }
    }
}
