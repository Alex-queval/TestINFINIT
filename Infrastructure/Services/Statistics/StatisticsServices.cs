using Infrastructure.Guards;
using Models.Data;

namespace Infrastructure.Services.Statistics
{
    /// <summary>
    /// Represents a service that gives statistics for <see cref="RepositoryInfo"/>.
    /// </summary>
    public sealed class StatisticsServices : IStatisticServices
    {
        /// <summary>
        /// Count number of word for each document and 
        /// set the result int <see cref="JsFileInfo.CountCharsInFile"/>.
        /// </summary>
        /// <param name="repositoryInfo">Repo's infos.</param>
        public void CountWordsForEachFile(RepositoryInfo repositoryInfo)
        {
            Guard.ArgumentNotNull(repositoryInfo, nameof(repositoryInfo));
            Guard.ArgumentItemsNotNull(repositoryInfo.JsFiles, nameof(repositoryInfo.JsFiles));

            foreach (var fileInfo in repositoryInfo.JsFiles)
            {
                foreach(var c in fileInfo.Content)
                {
                    if (char.IsLetter(c))
                    {
                        var letter = char.ToLowerInvariant(c);
                        if (!fileInfo.CharsCount.TryGetValue(letter, out int value))
                        {
                            value = 0;
                            fileInfo.CharsCount[letter] = value;
                        }
                        fileInfo.CharsCount[letter] = ++value;
                        fileInfo.TotalChars++;
                    }
                }
            }
        }

        /// <summary>
        /// Count the total number of chars for a repository
        /// set the result in <see cref="RepositoryInfo.CharsInTotal"/>
        /// </summary>
        /// <param name="repositoryInfo">Repo's infos.</param>
        public void CountWordForRepository(RepositoryInfo repositoryInfo)
        {
            Guard.ArgumentNotNull(repositoryInfo, nameof(repositoryInfo));
            Guard.ArgumentItemsNotNull(repositoryInfo.JsFiles, nameof(repositoryInfo.JsFiles));

            foreach (var fileInfo in repositoryInfo.JsFiles)
            {
                foreach (var c in fileInfo.Content)
                {
                    if (char.IsLetter(c))
                    {
                        var letter = char.ToLowerInvariant(c);
                        if (!repositoryInfo.TotalCharsCount.TryGetValue(letter, out int value))
                        {
                            value = 0;
                            repositoryInfo.TotalCharsCount[letter] = value;
                        }
                        repositoryInfo.TotalCharsCount[letter] = ++value;
                        repositoryInfo.TotalChars++;
                    }
                }
            }
        }
    }
}
