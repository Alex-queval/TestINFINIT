namespace Infrastructure.Services.FileFilter
{
    /// <summary>
    /// Represents a marker interface for a file filtering strategy.
    /// </summary>
    public interface IFileFilterStrategy
    {
        /// <summary>
        /// Determines if file match.
        /// </summary>
        /// <returns>True if match otherwise false.</returns>
        bool Match(string fileName);
    }
}
