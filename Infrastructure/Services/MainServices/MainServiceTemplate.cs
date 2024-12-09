using Microsoft.Extensions.Logging;
using Models.Data;

namespace Infrastructure.Services.MainServices
{
    /// <summary>
    /// Represents an abstract representation for the main service.
    /// </summary>
    public abstract class MainServiceTemplate : IMainService
    {
        protected readonly ILogger<MainServiceTemplate> _Logger;

        protected MainServiceTemplate(ILogger<MainServiceTemplate> logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Dispose the service when reach end.
        /// </summary>
        public void Dispose() => GC.SuppressFinalize(this);

        /// <summary>
        /// Execute the service.
        /// </summary>
        public async Task Execute()
        {
            try
            {
                var repositoryInfo = await RetrieveRepository();

                if (repositoryInfo == null)
                {
                    HandleNoRepository();
                    return;
                }

                await CollectData(repositoryInfo);
                ProcessData(repositoryInfo);
                OutputResults(repositoryInfo);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }

        }

        protected abstract Task<RepositoryInfo?> RetrieveRepository();
        protected abstract Task CollectData(RepositoryInfo repositoryInfo);
        protected abstract void ProcessData(RepositoryInfo repositoryInfo);
        protected abstract void OutputResults(RepositoryInfo repositoryInfo);


        /// <summary>
        /// Protected virtual, can be overrided if needed.
        /// </summary>
        protected virtual void HandleError(Exception ex)
        {
            _Logger.LogError($"An error occurred: {ex.Message}");
        }

        /// <summary>
        /// Protected virtual, can be overrided if needed.
        /// </summary>
        protected virtual void HandleNoRepository()
        {
            _Logger.LogWarning("No repository found.");
        }
    }
}
