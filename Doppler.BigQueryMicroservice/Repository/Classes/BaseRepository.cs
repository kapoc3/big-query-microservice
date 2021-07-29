using Doppler.BigQueryMicroservice.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Classes
{
    /// <summary>
    /// Base repository abstraction
    /// </summary>
    public abstract class BaseRepository
    {
        private readonly ILogger<BaseRepository> _logger;
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public BaseRepository(IDatabaseConnectionFactory connectionFactory, ILogger<BaseRepository> bigQueryLogger)
        {
            _connectionFactory = connectionFactory;
            _logger = bigQueryLogger;
        }


        protected async Task<IDbConnection> CreateConnectionAsync()
        {
            return await _connectionFactory.GetConnection();
        }

        public ILogger<BaseRepository> BigQueryLogger { get { return _logger; } }
    }
}
