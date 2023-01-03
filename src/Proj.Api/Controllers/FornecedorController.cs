﻿using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Proj.Api.ViewModels;
using System.Collections.Generic;

namespace Proj.Api.Controllers
{
    [Route("api/[controller]")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedorController(IFornecedorRepository fornecedorRepository, IMapper mapper, IFornecedorService fornecedorService)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> GetAll()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());

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
        public async Task<ActionResult<FornecedorViewModel>> Register([FromBody] FornecedorViewModel viewModel)
        {
            if(!ModelState.IsValid) return BadRequest();

            var fornecedorMapp = _mapper.Map<Fornecedor>(viewModel);

            var result = await _fornecedorService.Add(fornecedorMapp);

            if(!result) return BadRequest();

            return Ok(viewModel);
        }


    }
}