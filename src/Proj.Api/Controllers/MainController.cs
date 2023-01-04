using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Proj.Api.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly INotificador _notificador;

        public MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        /// <summary>
        /// Método que vai retornar todos os nossos erros de forma padronizada
        /// </summary>
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {

        }

        /// <summary>
        /// Acessa os erros do modelo
        /// </summary>
        protected void NotifyErrorModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string msg)
        {

        }

    }
}
