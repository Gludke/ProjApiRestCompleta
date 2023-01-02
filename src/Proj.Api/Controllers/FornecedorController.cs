using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Proj.Api.ViewModels;

namespace Proj.Api.Controllers
{
    [Route("api/[controller]")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        public FornecedorController(IFornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> GetAll()
        {


            return Ok();
        }
    }
}
