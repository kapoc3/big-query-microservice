using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Interfaces
{
    /// <summary>
    /// Contract specification for repository pattern
    /// </summary>
    /// <typeparam name="T">is a class with repository implementation</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}
