using Infrastructure.Services.GitHub;
using Infrastructure.Services.MainServices;
using Infrastructure.Services.Output;
using Infrastructure.Services.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Configurations;

namespace TestINFINIT
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var config = host.Services.GetRequiredService<IConfiguration>();

            Console.WriteLine($"App Name: {config["AppSettings:AppName"]}");
            Console.WriteLine($"Version: {config["AppSettings:Version"]}");

            var service = host.Services.GetRequiredService<IMainService>();

            await service.Execute();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Ajouter le fichier appsettings.json
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<RepositoryToSearchConfig>(context.Configuration.GetSection("RepositoryToSearchConfig"));
                    services.Configure<GitHubConnectionConfigs>(context.Configuration.GetSection("GitHubConnectionConfigs"));
                    services.AddTransient<IMainService, MainService>();
                    services.AddTransient<IGitHubServices, OctokitServices>();
                    services.AddTransient<IStatisticServices, StatisticsServices>();
                    services.AddTransient<IOutputServices, OutputConsole>();
                });
    }
}
