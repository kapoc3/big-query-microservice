using Dapper;
using Doppler.BigQueryMicroservice.Entitites;
using Doppler.BigQueryMicroservice.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Implementation
{
    /// <summary>
    /// UserAccessByUserrepository for access to sql database
    /// </summary>
    public class UserAccessByUserRepository : BaseRepository<UserAccessByUser>, IUserAccessByUserRepository
    {
        public UserAccessByUserRepository(IDatabaseConnectionFactory connectionFactory, ILogger<UserAccessByUser> bigQueryLogger) : base(connectionFactory, bigQueryLogger)
        {
            TableName = "[datastudio].[UserAccessByUser]";
        }

        #region IUserAccessByUserRepository methods implementations
        public async Task<IReadOnlyList<UserAccessByUser>> GetAllByUserIdAsync(int id)
        {
            var builder = new SqlBuilder();
            builder.Select("*").
                Where($"IdUser = @Id");

            var builderTemplate = builder.AddTemplate("Select /**select**/ from datastudio.UserAccessByUser /**where**/");

            using (var connection = await base.CreateConnectionAsync())
            {
                try
                {
                    var result = await connection.QueryAsync<UserAccessByUser>(builderTemplate.RawSql, new { Id = id });
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

        public async Task<bool> MergeEmailsAsync(int userId, List<string> emails)
        {
            using (var connection = await base.CreateConnectionAsync())
            {
                try
                {
                    var dt = new DataTable("[dbo].[TypeEmail]");
                    dt.Columns.Add("Email", typeof(string));

                    foreach (var email in emails)
                    {
                        dt.Rows.Add(email);
                    }

                    dt.SetTypeName("[dbo].[TypeEmail]");

                    string sql = @"MERGE [datastudio].[UserAccessByUser] t
USING @Emails s
ON
    (
        s.Email      = t.Email
        AND t.IdUser = @IdUser
    )
WHEN NOT MATCHED BY TARGET THEN
INSERT
    (IdUser
        , Email
        , CreatedAt
        , ValidFrom
        , ValidTo
        , UpdatedAt
    )
    VALUES
    (@IdUser
        , s.Email
        , GetDate()
        , GetDate ()
        , dateadd(year, 10, getdate())
        , GetDate()
    )
WHEN NOT MATCHED BY SOURCE
    AND
    (
        t.IdUser = @IdUser
    )
    THEN
    DELETE
    ;";

                    connection.Execute(sql, new { Emails = dt, IdUser = userId });
                    return true;
                }
                catch (Exception ex)
                {
                    base.BigQueryLogger.LogError(ex, $"Emails can not be merged for {nameof(userId)} {userId}");
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
