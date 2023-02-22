namespace Proj.Api.ViewModels.User
{
    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }
        public double? ExpiresIn { get; set; }
        public UserTokenViewModel UserToken { get; set; }

        public LoginResponseViewModel()
        {
            UserToken = new UserTokenViewModel();
        }

    }

    public class UserTokenViewModel
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public IEnumerable<ClaimViewModel> Claims { get; set; }

    }

    public class ClaimViewModel
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
