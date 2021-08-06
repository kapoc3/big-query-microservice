using Doppler.BigQueryMicroservice.Repository.Interfaces;

namespace Doppler.BigQueryMicroservice.Repository
{
    /// <summary>
    /// Contract for unit of work pattern
    /// </summary>
    public interface IUnitOfWork
    {
        IUserAccessByUserRepository UserAccessByUser { get; }
        IUserRepository User { get; }
    }
}
