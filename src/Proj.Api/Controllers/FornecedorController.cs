﻿using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proj.Api.Extensions;
using Proj.Api.ViewModels.Endereco;
using Proj.Api.ViewModels.Fornecedor;
using Proj.Business.Intefaces;

namespace Proj.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public FornecedorController(IFornecedorRepository fornecedorRepository,
                                    IMapper mapper,
                                    IFornecedorService fornecedorService,
                                    INotificador notificador,
                                    IEnderecoRepository enderecoRepository,
                                    IUserContext userContext) : base(notificador, userContext)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _enderecoRepository = enderecoRepository;
            _userContext = userContext;
        }


        [AllowAnonymous]//Permite o acesso da rota sem estar logado
        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAllFornecedorProdutosEndereco());

            return CustomResponse(fornecedores);
        }

        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> GetUnique(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.GetFornecedorProdutosEndereco(id));

            if(fornecedor == null) return NotFound();
            
            return CustomResponse(fornecedor);
        }

        [HttpGet("get/obterEndereco/{id:guid}")]
        public async Task<ActionResult<EnderecoViewModel>> GetAdress(Guid id)
        {
            var adress = _mapper.Map<EnderecoViewModel>(await _enderecoRepository.GetById(id));

            return CustomResponse(adress);
        }

        [HttpPost("register")]
        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        public async Task<IActionResult> Register([FromBody] AddFornecedorViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Add(CreateFornecedor(viewModel, _mapper));

            return CustomResponse(viewModel);
        }

        [HttpPut("update")]
        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        public async Task<IActionResult> Update([FromBody] UpdateFornecedorViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Update(_mapper.Map<Fornecedor>(viewModel));

            return CustomResponse(viewModel);
        }

        [HttpPut("update/endereco")]
        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        public async Task<IActionResult> UpdateAdress([FromBody] UpdateEnderecoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.UpdateAdress(_mapper.Map<Endereco>(viewModel));

            return CustomResponse(viewModel);
        }

        [HttpDelete("delete/{id:guid}")]
        [ClaimsAuthorize("Fornecedor", "Remover")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
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
