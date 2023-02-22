using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Proj.Api.Extensions;
using Proj.Api.ViewModels.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        //Classe criada contendo as configs do JWT
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
                return CustomResponse(await GerarJwt(viewModel.Email));
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
                return CustomResponse(await GerarJwt(viewModel.Email));
            
            if (result.IsLockedOut)//user bloqueado por tentativas
            {
                NotifyError("Usuário temporiariamente bloqueado por tentativas inválidas");
                return CustomResponse(viewModel);
            }
            
            NotifyError("Usuário ou senha incorretos");

            return CustomResponse(viewModel);
        }




        #region OTHER METHODS

        private async Task<LoginResponseViewModel> GerarJwt(string email)
        {
            var response = new LoginResponseViewModel();
            var userContext = await _userInManager.FindByEmailAsync(email);
            var userClaims = await _userInManager.GetClaimsAsync(userContext);
            var userRoles = await _userInManager.GetRolesAsync(userContext);

            //Passando outras claims essenciais
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, userContext.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, userContext.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));//Id do token
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));//"Não válido antes de"
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));//Data em que foi emitido

            //'Roles' são praticamente Claims
            foreach (var role in userRoles) userClaims.Add(new Claim("role", role));

            //Adicionando as claims no padrão exigido
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(userClaims);

            #region GERAÇÃO DO TOKEN

            //obj que manipula o JwtToken
            var tokenHandler = new JwtSecurityTokenHandler();

            //criando a chave criptografada em ASCII
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //criando o token em ASCII
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                //sempre UTC para n bugar caso o user esteja em outro lugar 
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                //credenciais de acesso
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            //serializando o token em JWT para web
            var encodedToken = tokenHandler.WriteToken(token);

            #endregion

            //Preenchendo o retorno - Só serve para o Front-End da aplicação
            response.AccessToken = encodedToken;
            response.ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds;
            response.UserToken.Id = userContext.Id;
            response.UserToken.Email = userContext.Email;
            response.UserToken.Claims = userClaims.Where(uc => FilterTypeClaims(uc.Type)).Select(uc => new ClaimViewModel { Type = uc.Type, Value = uc.Value });

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)//retorna os segundo relativos à data informada
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private bool FilterTypeClaims(string typeClaim)
        {
            return !typeClaim.Contains("sub") && !typeClaim.Contains("email") && !typeClaim.Contains("jti") && !typeClaim.Contains("nbf")
                    && !typeClaim.Contains("iat");
        }

        #endregion

    }
}
