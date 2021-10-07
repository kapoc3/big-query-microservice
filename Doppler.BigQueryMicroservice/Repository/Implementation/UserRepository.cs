using Dapper;
using Doppler.BigQueryMicroservice.Entitites;
using Doppler.BigQueryMicroservice.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Implementation
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
            builder.Select("u.IdUser").Select("u.Email").Select("l.Name as Language");
            builder.InnerJoin("[dbo].[Language] l on l.IdLanguage = u.IdLanguage");
            builder.Where($"Email = @AccountName");
            var builderTemplate = builder.AddTemplate($"Select /**select**/ from {TableName} u /**innerjoin**/ /**where**/");
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
