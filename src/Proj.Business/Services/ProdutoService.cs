using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public ProdutoService(IProdutoRepository produtoRepository,
                              INotificador notificador, 
                              IFornecedorRepository fornecedorRepository) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task Add(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            var fornecedorDb = await _fornecedorRepository.GetById(produto.FornecedorId);
            if (fornecedorDb == null)
            {
                Notificar("O fornecedor não existe");
                return;
            };

            await _produtoRepository.Add(produto);
            await _produtoRepository.SaveChanges();
        }

        public async Task Update(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Update(produto);
            await _produtoRepository.SaveChanges();
        }

        public async Task Remove(Guid id)
        {
            await _produtoRepository.Remove(id);
            await _produtoRepository.SaveChanges();
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }

    }
}