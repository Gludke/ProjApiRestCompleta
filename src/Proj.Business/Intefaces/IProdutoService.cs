using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IProdutoService : IDisposable
    {
        Task Add(Produto produto);
        Task Update(Produto produto);
        Task Remove(Guid id);
    }
}