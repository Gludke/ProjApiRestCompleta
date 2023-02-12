using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Proj.Api.Extensions;
using Proj.Api.ViewModels.User;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Proj.Api.Controllers
{
    [Route("api/conta")]
    public class AuthenticationController : MainController
    {
        //Classe do Identity - necessária para fazer login
        private readonly SignInManager<IdentityUser> _signInManager;
        //Classe do Identity - necessária para manipular o user (register, etc)
        private readonly UserManager<IdentityUser> _userInManager;
        private readonly AppSettings _appSettings;


        public AuthenticationController(INotificador notificador,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userInManager,
            IOptions<AppSettings> appSettings) : base(notificador)
        {
            _signInManager = signInManager;
            _userInManager = userInManager;
            _appSettings = appSettings.Value;
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
                return CustomResponse(GerarJwt());
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
                return CustomResponse(GerarJwt());
            }
            if (result.IsLockedOut)//user bloqueado por tentativas
            {
                NotifyError("Usuário temporiariamente bloqueado por tentativas inválidas");
                return CustomResponse(viewModel);
            }
            
            NotifyError("Usuário ou senha incorretos");

            return CustomResponse(viewModel);
        }




        #region OTHER METHODS

        private string GerarJwt()
        {
            //obj que manipula o JwtToken
            var tokenHandler = new JwtSecurityTokenHandler();

            //criando a chave criptografada em ASCII
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //criando o token em ASCII
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                //sempre UTC para n bugar caso o user esteja em outro lugar 
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                //credenciais de acesso
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            //serializando o token em JWT para web
            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        #endregion

    }
}
