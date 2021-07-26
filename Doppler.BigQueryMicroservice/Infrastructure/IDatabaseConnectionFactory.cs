using System.Data;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Infrastructure
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDbConnection> GetConnection();
    }
}
