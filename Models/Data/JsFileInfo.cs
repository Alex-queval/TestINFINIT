namespace Models.Data
{
    /// <summary>
    /// Represents informations of a js file.
    /// </summary>
    public class JsFileInfo(string name, string downloadPath)
    {

        /// <summary>
        /// Name of the file.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Download path.
        /// </summary>
        public string DownloadPath { get; set; } = downloadPath;

        /// <summary>
        /// Content of the file.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Number of chars in total.
        /// </summary>
        public int TotalChars { get; set; } = 0;

        /// <summary>
        /// Gives the number of char in the documents
        /// </summary>
        public SortedDictionary<char, int> CharsCount { get; set; } = [];
    }
}
