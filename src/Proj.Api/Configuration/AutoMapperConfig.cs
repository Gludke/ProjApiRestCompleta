using AutoMapper;
using DevIO.Business.Models;
using Proj.Api.ViewModels;

namespace Proj.Api.Configuration
{
    //Instalar package pelo Nuget: AutoMapper 12.0.0 -> AutoMapper.Extensions.Microsoft.DependencyInjection
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();
        }
    }
}
