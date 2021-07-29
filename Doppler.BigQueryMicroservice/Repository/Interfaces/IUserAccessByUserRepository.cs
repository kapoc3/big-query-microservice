using Doppler.BigQueryMicroservice.Entitites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Interfaces
{
    /// <summary>
    /// Extension of base contract for IUserAccessByUserRepository.
    /// </summary>
    public interface IUserAccessByUserRepository : IGenericRepository<UserAccessByUser>
    {
        Task<IReadOnlyList<UserAccessByUser>> GetAllByUserIdAsync(string accountName);
    }
}
