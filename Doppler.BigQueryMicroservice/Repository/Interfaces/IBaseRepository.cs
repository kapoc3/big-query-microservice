using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Interfaces
{
    /// <summary>
    /// Contract specification for repository pattern
    /// </summary>
    /// <typeparam name="T">is a class with repository implementation</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        //TODO add here the common methods for base repository
        Task<T> GetByIdAsync(int id);
    }
}
