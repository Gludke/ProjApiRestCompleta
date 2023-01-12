using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;

namespace DevIO.Business.Intefaces
{
    public interface IProdutoService : IDisposable
    {
        Task Add(Produto produto);
        Task Update(Produto produto, IFormFile imgProduto = null);
        Task Remove(Guid id);
    }
}