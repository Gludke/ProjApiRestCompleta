using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Proj.Api.ViewModels.Produto;
using Proj.Business.Utils;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddProdutoViewModel viewModel)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            //Trata a imagem enviada
            if (!String.IsNullOrEmpty(viewModel.ImagemUpload) && !String.IsNullOrEmpty(viewModel.Imagem))
            {
                viewModel.Imagem = $"{viewModel.Imagem}_{Guid.NewGuid()}{Path.GetExtension(viewModel.Imagem)}";
                
                Utils.UploadDocBase64(viewModel.ImagemUpload, viewModel.Imagem);
            }

            await _produtoService.Add(_mapper.Map<Produto>(viewModel));

            return CustomResponse(viewModel);
        }

        [HttpPost("registerFromForm")]
        public async Task<IActionResult> Register([FromForm] AddProdutoImageViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produto = _mapper.Map<Produto>(viewModel);

            //Trata a imagem enviada
            if (viewModel.ImagemUpload != null)
            {
                var imgName = $"{viewModel.ImagemUpload.FileName}_{Guid.NewGuid()}{Path.GetExtension(viewModel.ImagemUpload.FileName)}";
                produto.Imagem = imgName;

                Utils.UploadDocStream(viewModel.ImagemUpload.OpenReadStream(), imgName);
            }

            await _produtoService.Add(produto);

            return CustomResponse(viewModel);
        }



        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var prod = await _produtoRepository.GetById(id);
            if (prod == null) NotifyError("O produto não existe");

            await _produtoService.Remove(id);

            return CustomResponse(prod);
        }



    }
}
