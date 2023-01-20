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

        public async Task<IActionResult> Register(RegisterUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                EmailConfirmed = true,
            };

            //pass passado à parte porque o identity gera criptografia
            var result = await _userInManager.CreateAsync(user, viewModel.Password);


            return CustomResponse();
        }


    }
}
