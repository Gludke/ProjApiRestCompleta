using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        Task<IEnumerable<Fornecedor>> GetAllFornecedorProdutosEndereco();
        Task<Fornecedor> GetFornecedorEndereco(Guid id);
        Task<Fornecedor> GetFornecedorProdutosEndereco(Guid id);
    }
}