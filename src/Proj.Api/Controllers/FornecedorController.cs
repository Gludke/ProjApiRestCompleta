using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Proj.Api.ViewModels.Fornecedor;

namespace Proj.Api.Controllers
{
    [Route("api/[controller]")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedorController(IFornecedorRepository fornecedorRepository, IMapper mapper, IFornecedorService fornecedorService,
            INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> GetAll()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAllFornecedorProdutosEndereco());

            return Ok(fornecedores);
        }

        [HttpGet("get/{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> GetUnique(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.GetFornecedorProdutosEndereco(id));

            if(fornecedor == null) return NotFound();
            
            return Ok(fornecedor)
;       }

        [HttpPost("register")]
        public async Task<ActionResult<Fornecedor>> Register([FromBody] AddFornecedorViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Add(CreateFornecedor(viewModel, _mapper));

            return CustomResponse(viewModel);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Fornecedor>> Update([FromBody] UpdateFornecedorViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Update(_mapper.Map<Fornecedor>(viewModel));

            return CustomResponse(viewModel);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult<Fornecedor>> Delete([FromRoute] Guid id)
        {
            var fornecedorDb = await _fornecedorRepository.GetFornecedorEndereco(id);
            if (fornecedorDb == null) return NotFound();

            await _fornecedorService.Remove(fornecedorDb.Id);

            return CustomResponse();
        }


        #region METHODS

        private static Fornecedor CreateFornecedor(AddFornecedorViewModel viewModel, IMapper mapper)
        {
            var fornecedor = mapper.Map<Fornecedor>(viewModel);

            fornecedor.Endereco.FornecedorId = fornecedor.Id;
            foreach (var prod in fornecedor.Produtos) prod.FornecedorId = fornecedor.Id;

            return fornecedor;
        }

        #endregion


    }
}
