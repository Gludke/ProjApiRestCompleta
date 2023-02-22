using System.Security.Claims;

namespace Proj.Business.Intefaces
{
    public interface IUserContext
    {
        public string Name { get; }
        public Guid GetUserId();
        public string GetUserEmail();
        public bool IsAuthenticated();
        public bool IsInRole(string role);
        public IEnumerable<Claim> GetClaimsIdentity();
    }
}
