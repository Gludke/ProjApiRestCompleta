using DevIO.Business.Intefaces;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;

namespace Proj.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection service)
        {
            service.AddScoped<MeuDbContext>();
            
            service.AddScoped<IFornecedorRepository, FornecedorRepository>();

            service.AddScoped<IFornecedorService, FornecedorService>();

            return service;
        }
    }
}
