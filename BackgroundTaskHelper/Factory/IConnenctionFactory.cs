using System.Data;

namespace BackgroundTaskHelper.Factory
{
    public interface IConnenctionFactory
    {
        /// <summary>
        /// Gets the get connection.
        /// </summary>
        /// <value>The get connection.</value>
        IDbConnection GetConnection { get; }
    }
}
