using Infrastructure.Guards;
using Infrastructure.Services.GitHub;
using Infrastructure.Services.Output;
using Infrastructure.Services.Statistics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configurations;

namespace Infrastructure.Services.MainServices
{
    /// <summary>
    /// Represents a marker interface for the main service.
    /// </summary>
    public sealed class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IOptionsMonitor<RepositoryToSearchConfig> _Options;
        private readonly IGitHubServices _GitHubServices;
        private readonly IStatisticServices _StatisticsServices;
        private readonly IOutputServices _OutputServices;

        /// <summary>
        /// Initialize a new instance of <see cref="MainService"/>
        /// </summary>
        public MainService(IServiceProvider provider)
        {
            Guard.ArgumentNotNull(provider, nameof(provider));

            _logger = provider.GetRequiredService<ILogger<MainService>>();
            _Options = provider.GetRequiredService<IOptionsMonitor<RepositoryToSearchConfig>>();
            _GitHubServices = provider.GetRequiredService<IGitHubServices>();
            _StatisticsServices = provider.GetRequiredService<IStatisticServices>();
            _OutputServices = provider.GetRequiredService<IOutputServices>();
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task Execute()
        {
            try
            {
                var repositoryToSearch = _Options.CurrentValue;

                var repository = await _GitHubServices.TryGetRepository(repositoryToSearch.Owner, repositoryToSearch.RepositoryName);

                if (repository is not null)
                {
                    await _GitHubServices.GetJsFileInfos(repository);
                    await _GitHubServices.DonwloadFilesContents(repository);
                    _StatisticsServices.CountWordsForEachFile(repository);
                    _StatisticsServices.CountWordForRepository(repository);
                    _OutputServices.Output(repository);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexcpected error as occured: {ex.Message}");
                throw;
            }
        }
    }
}
