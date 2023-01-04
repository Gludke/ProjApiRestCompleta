using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
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
            service.AddScoped<IEnderecoRepository, EnderecoRepository>();
            service.AddScoped<IProdutoRepository, ProdutoRepository>();

            service.AddScoped<IFornecedorService, FornecedorService>();
            service.AddScoped<IProdutoService, ProdutoService>();

            service.AddScoped<INotificador, Notificador>();

            return service;
        }
    }
}
