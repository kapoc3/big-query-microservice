using Dapper;
using Doppler.BigQueryMicroservice.Infrastructure;
using Doppler.BigQueryMicroservice.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Classes
{
    /// <summary>
    /// Base repository abstraction
    /// </summary>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ILogger<T> _logger;
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public string TableName { get; set; }

        public BaseRepository(IDatabaseConnectionFactory connectionFactory, ILogger<T> bigQueryLogger)
        {
            _connectionFactory = connectionFactory;
            _logger = bigQueryLogger;
        }

        protected async Task<IDbConnection> CreateConnectionAsync()
        {
            return await _connectionFactory.GetConnection();
        }

        public ILogger<T> BigQueryLogger { get { return _logger; } }

        #region BaseRepository implementation
        public async Task<T> GetByIdAsync(int id)
        {
            var builder = new SqlBuilder();
            builder.Select("*").Where($"Id = @Id");
            var builderTemplate = builder.AddTemplate($"Select /**select**/ from {TableName} /**where**/");

            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<T>(builderTemplate.RawSql, new { Id = id });
                    return result;
                }
                catch (Exception ex)
                {
                    BigQueryLogger.LogError(ex, $"Entity {nameof(T)} by id not exist id:{id}");
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion
    }
}
