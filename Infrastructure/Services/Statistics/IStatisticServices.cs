using Models.Data;

namespace Infrastructure.Services.Statistics
{
    /// <summary>
    /// Represents a marker interface for a service that gives statistics for <see cref="RepositoryInfo"/>.
    /// </summary>
    public interface IStatisticServices
    {
        /// <summary>
        /// Count number of word for each document and 
        /// set the result int <see cref="JsFileInfo.CountCharsInFile"/>.
        /// </summary>
        /// <param name="repositoryInfo">Repo's infos.</param>
        void CountWordsForEachFile(RepositoryInfo repositoryInfo);

        /// <summary>
        /// Count the total number of chars for a repository
        /// set the result in <see cref="RepositoryInfo.CharsInTotal"/>
        /// </summary>
        /// <param name="repositoryInfo">Repo's infos.</param>
        void CountWordForRepository(RepositoryInfo repositoryInfo);
    }
}
