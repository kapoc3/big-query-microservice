using Dapper;
using Doppler.BigQueryMicroservice.Entitites;
using Doppler.BigQueryMicroservice.Infrastructure;
using Doppler.BigQueryMicroservice.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Classes
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseConnectionFactory connectionFactory, ILogger<User> bigQueryLogger) : base(connectionFactory, bigQueryLogger)
        {
            TableName = "[Dbo].[User]";
        }
        public async Task<User> GetUserByEmail(string accountName)
        {
            var builder = new SqlBuilder();
            builder.Select("IdUser").Select("Email").Where($"Email = @AccountName");
            var builderTemplate = builder.AddTemplate($"Select /**select**/ from {TableName} /**where**/");
            using (var connection = await CreateConnectionAsync())
            {
                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<User>(builderTemplate.RawSql, new { AccountName = accountName });
                    return result;
                }
                catch (Exception ex)
                {
                    BigQueryLogger.LogError(ex, $"Entity {nameof(User)} by accountName not exist accountName:{accountName}");
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
