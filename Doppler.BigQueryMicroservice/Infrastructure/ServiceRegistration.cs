using Doppler.BigQueryMicroservice.Repository;
using Doppler.BigQueryMicroservice.Repository.Implementation;
using Doppler.BigQueryMicroservice.Services;
using Doppler.BigQueryMicroservice.Services.Implementation;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doppler.BigQueryMicroservice.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUserAccessByUserRepository, UserAccessByUserRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            services.AddTransient<IEmailSender, RelayEmailSender>();
            services.AddTransient<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
        }
    }
}
