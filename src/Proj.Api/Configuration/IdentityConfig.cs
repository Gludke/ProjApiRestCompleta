using Microsoft.AspNetCore.Identity;
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

            //Adicionando o serviço do Identity
            //Pode ser necessário o package 'Microsoft.AspNetCore.Identity.UI'
            //É possível adicionar qualquer classe que extenda as interfaces corretas no lugar dessas classes genéricas do Identity
            services.AddDefaultIdentity<IdentityUser>()
                //tabela de claims dos users
                .AddRoles<IdentityRole>()
                //define o uso do EF Core                                           
                .AddEntityFrameworkStores<AplicationDbContext>()
                //Recurso que gera tokens, mas não é necessário
                .AddDefaultTokenProviders();



            return services;
        }
    }
}
