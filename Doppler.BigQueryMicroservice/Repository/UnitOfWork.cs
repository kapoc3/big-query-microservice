using Doppler.BigQueryMicroservice.Repository.Interfaces;

namespace Doppler.BigQueryMicroservice.Repository
{
    /// <summary>
    /// Unit of work pattern implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IUserAccessByUserRepository productRepository, IUserRepository userRepository)
        {
            UserAccessByUser = productRepository;
            User = userRepository;
        }

        public IUserAccessByUserRepository UserAccessByUser { get; }

        public IUserRepository User { get; }
    }
}
