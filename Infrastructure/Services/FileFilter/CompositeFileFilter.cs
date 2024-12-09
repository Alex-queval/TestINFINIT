namespace Infrastructure.Services.FileFilter
{
    /// <summary>
    /// Represents a container for filtering file that match any <see cref="IFileFilterStrategy"/>
    /// </summary>
    public class CompositeFileFilter : IFileFilterStrategy
    {
        private readonly IEnumerable<IFileFilterStrategy> _Strategies;

        /// <summary>
        /// Initialize a new instance of <see cref="CompositeFileFilter"/>.
        /// </summary>
        /// <param name="strategies"></param>
        public CompositeFileFilter(IEnumerable<IFileFilterStrategy> strategies)
        {
            _Strategies = strategies;
        }

        /// <summary>
        /// Match any of <see cref="IFileFilterStrategy"/>
        /// </summary>
        public bool Match(string fileName)
        {
            return _Strategies.Any(strategy => strategy.Match(fileName));
        }
    }
}
