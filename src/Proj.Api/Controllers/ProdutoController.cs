using AutoMapper;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Proj.Api.ViewModels.Produto;

namespace Proj.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProdutoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutoController(INotificador notificador,
                                 IProdutoRepository produtoRepository,
                                 IProdutoService produtoService,
                                 IMapper mapper) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());

            return CustomResponse(produtos);
        }

        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            if (produto == null) NotifyError("O produto não existe");

            return CustomResponse(produto);
        }

        

    }
}
