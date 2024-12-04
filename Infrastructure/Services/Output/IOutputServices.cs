using Models.Data;

namespace Infrastructure.Services.Output
{
    /// <summary>
    /// Represents a marker interface for a service 
    /// that output statistics of <see cref="RepositoryInfo"/>.
    /// </summary>
    public interface IOutputServices
    {
        /// <summary>
        /// Output statistics
        /// </summary>
        /// <param name="repositoryInfo">Repo's info.</param>
        void Output(RepositoryInfo repositoryInfo);
    }
}
