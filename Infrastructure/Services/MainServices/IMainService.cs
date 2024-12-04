namespace Infrastructure.Services.MainServices
{
    /// <summary>
    /// Represents a marker interface for the main service.
    /// </summary>
    public interface IMainService : IDisposable
    {
        /// <summary>
        /// Execute the service.
        /// </summary>
        Task Execute();
    }
}
