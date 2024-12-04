using Models.Data;

namespace Infrastructure.Services.GitHub
{
    /// <summary>
    /// Represents a marker interface for a service that manage action to GitHub
    /// </summary>
    public interface IGitHubServices
    {
        /// <summary>
        /// Try get repository informations.
        /// </summary>
        /// <param name="owner">Repo's owner name</param>
        /// <param name="repositoryName">Repo's name</param>
        /// <returns>Repository info <see cref="RepositoryInfo"/></returns>
        Task<RepositoryInfo?> TryGetRepository(string owner, string repositoryName);

        /// <summary>
        /// Get all info of js or typescript files (.js, .ts, .jsx, and .tsx)
        /// of a specific repository and set into <see cref="RepositoryInfo.JsFiles"/>.
        /// </summary>
        /// <param name="repositoryInfo">Repo's info</param>
        Task GetJsFileInfos(RepositoryInfo repositoryInfo);

        /// <summary>
        /// Download all files content using <see cref="HttpClient"/> and 
        /// add content to <see cref="JsFileInfo.Content"/>
        /// </summary>
        /// <param name="repositoryInfo">Repo's info</param>
        Task DonwloadFilesContents(RepositoryInfo repositoryInfo);
    }
}
