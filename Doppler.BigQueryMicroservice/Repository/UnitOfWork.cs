using Doppler.BigQueryMicroservice.Repository.Interfaces;

namespace Doppler.BigQueryMicroservice.Repository
{
    /// <summary>
    /// Unit of work pattern implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IUserAccessByUserRepository productRepository)
        {
            UserAccessByUser = productRepository;
        }

        public IUserAccessByUserRepository UserAccessByUser { get; }
    }
}
