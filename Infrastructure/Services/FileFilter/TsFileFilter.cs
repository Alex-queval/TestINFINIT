namespace Infrastructure.Services.FileFilter
{
    /// <summary>
    /// Represents a strategy for filtering file ts or tsx file.
    /// </summary>
    public class TsFileFilter : IFileFilterStrategy
    {
        /// <summary>
        /// Match on the extension of the file.
        /// </summary>
        /// <returns>True if file ends with .ts or .tsx, false otherwise.</returns>
        public bool Match(string fileName) =>
            fileName.EndsWith(".ts", StringComparison.OrdinalIgnoreCase) ||
            fileName.EndsWith(".tsx", StringComparison.OrdinalIgnoreCase);
    }
}
