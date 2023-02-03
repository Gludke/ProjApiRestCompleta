using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proj.Api.ViewModels.User;

namespace Proj.Api.Controllers
{
    [Route("api/conta")]
    public class AuthenticationController : MainController
    {
        //Classe do Identity - necessária para fazer login
        private readonly SignInManager<IdentityUser> _signInManager;
        //Classe do Identity - necessária para manipular o user (register, etc)
        private readonly UserManager<IdentityUser> _userInManager;


        public AuthenticationController(INotificador notificador, 
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userInManager) : base(notificador)
        {
            _signInManager = signInManager;
            _userInManager = userInManager;
        }

        [HttpPost("nova")]
        public async Task<IActionResult> Register(RegisterUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                EmailConfirmed = true,
            };

            //'pass' passado à parte porque o identity gera criptografia
            var result = await _userInManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return CustomResponse(viewModel);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    NotifyError(error.Description);
                }
            }

            return CustomResponse(viewModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            //'true' - bloqueia o login do user em caso de 5 erros por alguns minutos
            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false, true);
            if (result.Succeeded)
            {
                return CustomResponse(viewModel);
            }
            if (result.IsLockedOut)//user bloqueado por tentativas
            {
                NotifyError("Usuário temporiariamente bloqueado por tentativas inválidas");
                return CustomResponse(viewModel);
            }
            
            NotifyError("Usuário ou senha incorretos");

            return CustomResponse(viewModel);
        }


    }
}
