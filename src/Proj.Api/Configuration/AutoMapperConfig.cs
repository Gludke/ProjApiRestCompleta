using AutoMapper;
using DevIO.Business.Models;
using Proj.Api.ViewModels.Endereco;
using Proj.Api.ViewModels.Fornecedor;
using Proj.Api.ViewModels.Produto;

namespace Proj.Api.Configuration
{
    //Instalar package pelo Nuget: AutoMapper 12.0.0 -> AutoMapper.Extensions.Microsoft.DependencyInjection
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, AddFornecedorViewModel>().ReverseMap();
            CreateMap<Fornecedor, UpdateFornecedorViewModel>().ReverseMap();
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();

            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Endereco, AddEnderecoViewModel>().ReverseMap();
            CreateMap<Endereco, UpdateEnderecoViewModel>().ReverseMap();

            CreateMap<Produto, AddProdutoViewModel>().ReverseMap();
            CreateMap<Produto, AddProdutoModelBinderImageViewModel>().ReverseMap();
            CreateMap<Produto, UpdateProdutoViewModel>().ReverseMap();
            CreateMap<ProdutoViewModel, Produto>();
            CreateMap<Produto, ProdutoViewModel>()//pegando o nome do 'Fornecedor' e mapeando dentro da var de 'ProdutoViewModel'
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(prod => prod.Fornecedor.Nome));
            CreateMap<Produto, AddProdutoImageViewModel>().ReverseMap();



        }
    }
}
