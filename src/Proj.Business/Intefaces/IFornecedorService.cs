using DevIO.Business.Models;
using Proj.Api.ViewModels.Fornecedor;

namespace DevIO.Business.Intefaces
{
    public interface IFornecedorService : IDisposable
    {
        Task<bool> Add(AddFornecedorViewModel fornecedor);
        Task<bool> Update(Fornecedor fornecedor);
        Task<bool> Remove(Guid id);

        Task UpdateAdress(Endereco endereco);
    }
}