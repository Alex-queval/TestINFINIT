namespace Models.Configurations
{
    /// <summary>
    /// Represents a configuration file for GitHub.
    /// </summary>
    public class GitHubConnectionConfigs
    {
        /// <summary>
        /// This is your personnal GitHub token.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
