namespace Models.Configurations
{
    /// <summary>
    /// Represents a configuration for repository to search.
    /// </summary>
    public class RepositoryToSearchConfig
    {
        /// <summary>
        /// Owner name.
        /// </summary>
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// Repository name.
        /// </summary>
        public string RepositoryName { get; set; } = string.Empty;
    }
}
