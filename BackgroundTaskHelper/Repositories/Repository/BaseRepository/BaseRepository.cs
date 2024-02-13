using BackgroundTaskHelper.Controllers;
using BackgroundTaskHelper.Factory;
using System.Data;
using static Hangfire.Storage.JobStorageFeatures;

namespace BackgroundTaskHelper.Repositories.Repository.BaseRepository
{
    public class BaseRepository : IBaseRepository
    {
        private bool disposedValue;
        private readonly ILogger<BaseRepository> _logger;

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        protected IDbConnection Connection;
        protected BaseRepository(IConnenctionFactory connectionFactory, ILogger<BaseRepository> logger)
        {
            _logger = logger;
            try
            {
                Connection = connectionFactory.GetConnection;
                //Not required to open the connection, it will automatically managed by DAPPER
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Connection?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~BaseRepository()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
