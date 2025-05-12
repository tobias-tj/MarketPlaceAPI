using Application.Interfaces.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using WebAPI.Helpers;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<DbConnection>();

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<JwtHelper>();
            services.AddSingleton<IAuthRepository, AuthRepository>();
        }
    }
}
