using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IFornecedorService : IDisposable
    {
        Task<bool> Add(Fornecedor fornecedor);
        Task<bool> Update(Fornecedor fornecedor);
        Task<bool> Remove(Guid id);

        Task UpdateAdress(Endereco endereco);
    }
}