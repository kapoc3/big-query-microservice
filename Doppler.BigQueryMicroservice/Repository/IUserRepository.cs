using Doppler.BigQueryMicroservice.Entitites;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository
{
    /// <summary>
    /// Extension of base contract
    /// for User entity.
    /// </summary>
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmail(string accountName);
    }
}
