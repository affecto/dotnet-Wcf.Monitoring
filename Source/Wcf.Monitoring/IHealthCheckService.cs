namespace Affecto.Wcf.Monitoring
{
    /// <summary>
    /// Service interface for implementing a deep system health check
    /// </summary>
    public interface IHealthCheckService
    {
        /// <summary>
        /// Check system health. Throws an exception if the system is not healthy.
        /// </summary>
        void CheckHealth();
    }
}