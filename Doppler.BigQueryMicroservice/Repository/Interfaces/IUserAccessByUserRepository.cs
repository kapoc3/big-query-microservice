using Doppler.BigQueryMicroservice.Entitites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Interfaces
{
    /// <summary>
    /// Extension of base contract for IUserAccessByUserRepository.
    /// </summary>
    public interface IUserAccessByUserRepository : IBaseRepository<UserAccessByUser>
    {
        Task<IReadOnlyList<UserAccessByUser>> GetAllByUserIdAsync(string accountName);
        Task<bool> MergeEmailsAsync(int userId, List<string> emails);
    }
}
