using Dapper;
using Doppler.BigQueryMicroservice.Entitites;
using Doppler.BigQueryMicroservice.Infrastructure;
using Doppler.BigQueryMicroservice.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Classes
{
    /// <summary>
    /// UserAccessByUserrepository for access to sql database
    /// </summary>
    public class UserAccessByUserRepository : BaseRepository, IUserAccessByUserRepository
    {
        public UserAccessByUserRepository(IDatabaseConnectionFactory connectionFactory, ILogger<BaseRepository> bigQueryLogger) : base(connectionFactory, bigQueryLogger) { }

        #region BaseRepository implementation
        Task<int> IGenericRepository<UserAccessByUser>.AddAsync(UserAccessByUser entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IGenericRepository<UserAccessByUser>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyList<UserAccessByUser>> IGenericRepository<UserAccessByUser>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<UserAccessByUser> IGenericRepository<UserAccessByUser>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<int> IGenericRepository<UserAccessByUser>.UpdateAsync(UserAccessByUser entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserAccessByUserRepository methods implementations
        public async Task<IReadOnlyList<UserAccessByUser>> GetAllByUserIdAsync(string accountName)
        {
            var builder = new SqlBuilder();
            builder.Select("uabu.*").
                InnerJoin("dbo.\"user\" u on u.IdUser = uabu.IdUser").
                Where($"u.email = '{accountName}'");

            var builderTemplate = builder.AddTemplate("Select /**select**/ from datastudio.UserAccessByUser uabu /**innerjoin**/ /**where**/");

            using (var connection = await base.CreateConnectionAsync())
            {
                try
                {
                    var result = await connection.QueryAsync<UserAccessByUser>(builderTemplate.RawSql);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    base.BigQueryLogger.LogError(ex.Message);
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
