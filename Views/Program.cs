using Microsoft.Extensions.DependencyInjection;

namespace Views
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") })
                .AddTransient(typeof(ApiClient<>))
                .BuildServiceProvider();

            var ui = new UserInterface(serviceProvider);
            await ui.RunAsync();
        }
    }
}
