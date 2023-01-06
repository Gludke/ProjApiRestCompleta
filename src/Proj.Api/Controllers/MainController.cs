using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
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

        protected bool OperationValid()
        {
            return !_notificador.TemNotificacao();
        }

        /// <summary>
        /// Resposta final da chamada de uma rota
        /// </summary>
        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationValid())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            }); ;
        }

        /// <summary>
        /// Método que vai retornar todos os nossos ERROS de forma padronizada
        /// </summary>
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            //'ModelStateDictionary' é o objeto que armazena os erros de modelo gerados pelo próprio sistema, não pela
            //camada de negócios.
            if (!modelState.IsValid) NotifyErrorModelInvalid(modelState);

            return CustomResponse();
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
            _notificador.Handle(new Notificacao(msg));
        }

    }
}
