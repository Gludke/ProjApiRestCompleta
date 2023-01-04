using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> GetAdressByFornecedorId(Guid fornecedorId);
        void RemoveByFornecedorId(Guid fornecedorId);
    }
}