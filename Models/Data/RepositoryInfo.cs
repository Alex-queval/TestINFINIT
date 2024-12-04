namespace Models.Data
{
    /// <summary>
    /// Represents info of the repotisory
    /// </summary>
    public class RepositoryInfo(string owner, string repositoryName)
    {
        /// <summary>
        /// Repo's owner name.
        /// </summary>
        public string Owner { get; set; } = owner;

        /// <summary>
        /// Repo's name.
        /// </summary>
        public string RepositoryName { get; set; } = repositoryName;

        /// <summary>
        /// List of <see cref="JsFileInfo"/>.
        /// </summary>
        public List<JsFileInfo> JsFiles { get; set; } = [];

        /// <summary>
        /// Number of Jsfile.
        /// </summary>
        public int TotalJsFile { get; set; } = 0;

        /// <summary>
        /// Number of chars in total.
        /// </summary>
        public int TotalChars { get; set; } = 0;

        /// <summary>
        /// Number of different chars.
        /// </summary>
        public SortedDictionary<char, int> TotalCharsCount { get; set; } = [];
    }
}
