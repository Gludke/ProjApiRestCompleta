using Proj.Business.Intefaces;
using System.Security.Claims;

namespace Proj.Api.Extensions
{
    public class AspNetUser : IUserContext
    {
        //A prop está sendo instanciada apenas aqui, na camdada API. Devemos evitar colocar a lib 'AspNetCore' na camada 'Business',
        //por isso estamos fazendo tudo aqui na camada de API.
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        //No Asp.Net precisa ser feito o GetUserId dessa maneira. O Id do User logado está sendo obtido da sua lista de claims
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        //No Asp.Net precisa ser feito o GetUserEmail dessa maneira. O Email do User logado está sendo obtido da sua lista de claims
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
