using Doppler.BigQueryMicroservice.Entitites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository
{
    /// <summary>
    /// Extension of base contract for IUserAccessByUserRepository.
    /// </summary>
    public interface IUserAccessByUserRepository : IBaseRepository<UserAccessByUser>
    {
        Task<IReadOnlyList<UserAccessByUser>> GetAllByUserIdAsync(int id);
        Task<bool> MergeEmailsAsync(int userId, List<string> emails);
    }
}
