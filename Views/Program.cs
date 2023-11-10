using Microsoft.Extensions.DependencyInjection;

namespace Views
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(new HttpClient())
                .AddSingleton<ApiClient>(provider =>
                    new ApiClient(provider.GetRequiredService<HttpClient>(), "https://localhost:5001"))
                .BuildServiceProvider();

            var apiService = serviceProvider.GetRequiredService<ApiClient>();
            var ui = new UserInterface(apiService);
            await ui.RunAsync();
        }
    }
}
