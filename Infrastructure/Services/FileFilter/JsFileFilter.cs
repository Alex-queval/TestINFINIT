namespace Infrastructure.Services.FileFilter
{
    /// <summary>
    /// Represents a strategy for filtering file js or jsx file.
    /// </summary>
    public class JsFileFilter : IFileFilterStrategy
    {
        /// <summary>
        /// Match on the extension of the file.
        /// </summary>
        /// <returns>True if file ends with .js or .jsx, false otherwise.</returns>
        public bool Match(string fileName) =>
            fileName.EndsWith(".js", StringComparison.OrdinalIgnoreCase) ||
            fileName.EndsWith(".jsx", StringComparison.OrdinalIgnoreCase);
    }
}
