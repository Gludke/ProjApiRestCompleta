using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using Microsoft.AspNetCore.Http;
using Proj.Business.Utils;

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
                NotifyError("O fornecedor não existe");
                return;
            };

            await _produtoRepository.Add(produto);
            await _produtoRepository.SaveChanges();
        }

        public async Task Update(Produto produto, IFormFile imgProduto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            var produtoDb = await _produtoRepository.GetById(produto.Id);
            if(produtoDb == null)
            {
                NotifyError("O produto não existe");
                return;
            }

            //Se há nova imagem, remover a antiga e salvar a nova
            if (imgProduto != null)
            {
                var nameNewDoc = $"{imgProduto.FileName}_{Guid.NewGuid()}{Path.GetExtension(imgProduto.FileName)}";

                Utils.DeleteDocument(produtoDb.Imagem);

                Utils.UploadDocStream(imgProduto.OpenReadStream(), nameNewDoc);
                produtoDb.Imagem = nameNewDoc;
            }

            produtoDb.Update(produto);

            await _produtoRepository.Update(produtoDb);
            await _produtoRepository.SaveChanges();
        }

        public async Task Remove(Guid id)
        {
            var prod = await _produtoRepository.GetById(id);
            if (prod == null)
            {
                NotifyError("O produto não existe");
                return;
            }

            _produtoRepository.Remove(prod);
            await _produtoRepository.SaveChanges();
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }

    }
}