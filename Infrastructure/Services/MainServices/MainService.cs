using Infrastructure.Guards;
using Infrastructure.Services.GitHub;
using Infrastructure.Services.Output;
using Infrastructure.Services.Statistics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configurations;
using Models.Data;

namespace Infrastructure.Services.MainServices
{
    /// <summary>
    /// Represents the main service.
    /// </summary>
    public class MainService : MainServiceTemplate
    {
        private readonly IOptionsMonitor<RepositoryToSearchConfig> _Options;
        private readonly IGitHubServices _GitHubServices;
        private readonly IStatisticServices _StatisticsServices;
        private readonly IOutputServices _OutputServices;

        /// <summary>
        /// Initialize a new instance of <see cref="MainService"/>
        /// </summary>
        public MainService(IServiceProvider provider) : base(provider.GetRequiredService<ILogger<MainService>>())
        {
            Guard.ArgumentNotNull(provider, nameof(provider));

            _Options = provider.GetRequiredService<IOptionsMonitor<RepositoryToSearchConfig>>();
            _GitHubServices = provider.GetRequiredService<IGitHubServices>();
            _StatisticsServices = provider.GetRequiredService<IStatisticServices>();
            _OutputServices = provider.GetRequiredService<IOutputServices>();
        }

        protected override async Task<RepositoryInfo?> RetrieveRepository()
        {
            var repositoryToSearch = _Options.CurrentValue;
            return await _GitHubServices.TryGetRepository(repositoryToSearch.Owner, repositoryToSearch.RepositoryName);
        }

        protected override async Task CollectData(RepositoryInfo repositoryInfo)
        {
            await _GitHubServices.GetJsFileInfos(repositoryInfo);
            await _GitHubServices.DonwloadFilesContents(repositoryInfo);
        }

        protected override void ProcessData(RepositoryInfo repositoryInfo)
        {
            _StatisticsServices.CountWordsForEachFile(repositoryInfo);
            _StatisticsServices.CountWordForRepository(repositoryInfo);
        }

        protected override void OutputResults(RepositoryInfo repositoryInfo)
        {
            _OutputServices.Output(repositoryInfo);
        }
    }
}
