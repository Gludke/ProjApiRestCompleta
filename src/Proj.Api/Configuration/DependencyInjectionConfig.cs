using DevIO.Business.Intefaces;
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

            return service;
        }
    }
}
