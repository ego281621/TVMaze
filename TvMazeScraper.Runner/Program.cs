using System.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMaze.ApiClient;
using TvMaze.Data;
using TvMaze.Scraper.PolicyProvider;

namespace TvMaze.Scraper.Runner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var scraper = serviceProvider.GetRequiredService<TvMazeScraper>();

            await scraper.RunAsync();

            serviceProvider.Dispose();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<TvMazeScraper>();

            services.AddTransient<TvMazeRepository>();

            services.AddTransient<TvMazeApiClient>();

            services.AddHttpClient<TvMazeApiClient>()
                .AddPolicyHandler(RetryPolicyProvider.Get());

            services.AddLogging(builder => builder.AddConsole());
            services.AddSingleton(configuration);
        }
    }
}
