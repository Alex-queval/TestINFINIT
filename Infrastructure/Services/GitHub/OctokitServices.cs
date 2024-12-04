using Infrastructure.Guards;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configurations;
using Models.Data;
using Octokit;

namespace Infrastructure.Services.GitHub
{
    /// <summary>
    /// Represents a service that manage action to GitHub using octokit
    /// <see cref="https://github.com/octokit/octokit.net"/>
    /// </summary>
    public class OctokitServices : IGitHubServices
    {
        private readonly ILogger<OctokitServices> _Logger;
        private readonly IOptionsMonitor<GitHubConnectionConfigs> _GitHubConfigs;

        private GitHubClient _Client { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="OctokitServices"/>.
        /// </summary>
        public OctokitServices(IServiceProvider provider)
        {
            Guard.ArgumentNotNull(provider, nameof(provider));
            _Logger = provider.GetRequiredService<ILogger<OctokitServices>>();
            _GitHubConfigs = provider.GetRequiredService<IOptionsMonitor<GitHubConnectionConfigs>>();

            var currentValue = _GitHubConfigs.CurrentValue;
            _Client = CreateClient(currentValue.Token);
        }

        /// <summary>
        /// Try get repository informations.
        /// </summary>
        /// <param name="owner">Repo's owner name</param>
        /// <param name="repositoryName">Repo's name</param>
        /// <returns>Repository info <see cref="RepositoryInfo"/></returns>
        public async Task<RepositoryInfo?> TryGetRepository(string owner, string repositoryName)
        {
            Guard.ArgumentNotNullOrEmpty(owner, nameof(owner));
            Guard.ArgumentNotNullOrEmpty(repositoryName, nameof(repositoryName));

            try
            {
                _ = await GetRepository(owner, repositoryName);
                return new RepositoryInfo(owner, repositoryName);
            }
            catch (NotFoundException)
            {
                _Logger.LogError($"Repository {owner}/{repositoryName} does not exists.");
                return null;
            }
            catch (AuthorizationException)
            {
                _Logger.LogError($"You have no authorization to acces to {owner}/{repositoryName} repository.");
                return null;
            }
            catch (RateLimitExceededException)
            {
                _Logger.LogError($"You have no exceeded the number of request to the GitHub API.");
                return null;
            }
            catch (Exception ex)
            {
                _Logger.LogError($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get all info of js or typescript files (.js, .ts, .jsx, and .tsx)
        /// of a specific repository and set into <see cref="RepositoryInfo.JsFiles"/>.
        /// </summary>
        /// <param name="repositoryInfo">Repo's info</param>
        public async Task GetJsFileInfos(RepositoryInfo repositoryInfo)
        {
            Guard.ArgumentNotNull(repositoryInfo, nameof(repositoryInfo));

            var jsFilesInfos = await GetContentRecursive(repositoryInfo);
            repositoryInfo.TotalJsFile = jsFilesInfos.Count;
            repositoryInfo.JsFiles = jsFilesInfos;
        }

        /// <summary>
        /// Download all files content using <see cref="HttpClient"/> and 
        /// add content to <see cref="JsFileInfo.Content"/>
        /// </summary>
        /// <param name="repositoryInfo">Repo's info</param>
        public async Task DonwloadFilesContents(RepositoryInfo repositoryInfo)
        {
            using var httpClient = new HttpClient();
            foreach (JsFileInfo fileInfo in repositoryInfo.JsFiles)
            {
                fileInfo.Content = await httpClient.GetStringAsync(fileInfo.DownloadPath);
            }
        }

        /// <summary>
        /// Use this method to create the client.
        /// </summary>
        /// <param name="token">Your personal acces token to interact with GitHub</param>
        private static GitHubClient CreateClient(string token)
        {
            Guard.ArgumentNotNullOrEmpty(token, nameof(token));

            return new GitHubClient(new ProductHeaderValue("LodashAnalyzer"))
            {
                Credentials = new Credentials(token)
            };
        }

        private async Task<List<JsFileInfo>> GetContentRecursive(RepositoryInfo repositoryInfo, string path = "")
        {
            List<JsFileInfo> jsFileInfos = new();

            try
            {
                var contents = await GetContents(repositoryInfo.Owner, repositoryInfo.RepositoryName, path);
                foreach (var content in contents)
                {
                    if (content.Type == ContentType.File)
                    {
                        if (content.Name.EndsWith(".js", StringComparison.OrdinalIgnoreCase) ||
                            content.Name.EndsWith(".ts", StringComparison.OrdinalIgnoreCase) ||
                            content.Name.EndsWith(".jsx", StringComparison.OrdinalIgnoreCase) ||
                            content.Name.EndsWith(".tsx", StringComparison.OrdinalIgnoreCase))
                        {
                            jsFileInfos.Add(new JsFileInfo(content.Name, content.DownloadUrl));
                        }
                    }
                    else if (content.Type == ContentType.Dir)
                    {
                        var subdirectoryFiles = await GetContentRecursive(repositoryInfo, content.Path);
                        jsFileInfos.AddRange(subdirectoryFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Cannot access to repository ({repositoryInfo.Owner}/{repositoryInfo.RepositoryName}, reeason : {ex.Message}");
            }

            return jsFileInfos;
        }

        /// <summary>
        /// Protected virtual for unit testing.
        /// </summary>
        protected virtual async Task<Repository> GetRepository(string owner, string repositoryName)
        {
            return await _Client.Repository.Get(owner, repositoryName);
        }

        protected virtual async Task<List<RepositoryContent>> GetContents(string owner, string repositoryName, string path)
        {
            IReadOnlyList<RepositoryContent> contents;

            if (string.IsNullOrEmpty(path))
            {
                contents = await _Client.Repository.Content.GetAllContents(owner, repositoryName);
            }
            else
            {
                contents = await _Client.Repository.Content.GetAllContents(owner, repositoryName, path);
            }
            return contents.ToList();
        }
    }
}
