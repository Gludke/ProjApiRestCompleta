using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using Proj.Api.Extensions;
using Proj.Business.Intefaces;

namespace Proj.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection service)
        {
            service.AddScoped<MeuDbContext>();

            //'AddSingleton' - mesma instância para todos os users logados. O .Net, nesse caso, não confunde os contextos.
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            service.AddScoped<IFornecedorRepository, FornecedorRepository>();
            service.AddScoped<IEnderecoRepository, EnderecoRepository>();
            service.AddScoped<IProdutoRepository, ProdutoRepository>();

            service.AddScoped<IFornecedorService, FornecedorService>();
            service.AddScoped<IProdutoService, ProdutoService>();

            service.AddScoped<INotificador, Notificador>();

            service.AddScoped<IUserContext, AspNetUser>();

            return service;
        }
    }
}
