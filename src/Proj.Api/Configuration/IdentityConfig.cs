using Microsoft.EntityFrameworkCore;
using Proj.Api.Data;

namespace Proj.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Usando um contexto diferente apenas para o Identity, mas usando a mesma connection string do DB do sistema, portanto o mesmo DB
            services.AddDbContext<AplicationDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));






            return services;
        }
    }
}
