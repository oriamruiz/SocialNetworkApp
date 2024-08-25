using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.ViewModels.Users;

namespace SocialNetworkApp.MiddleWare
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContext;

        public ValidateUserSession(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool HasUser()
        {
            UserViewModel userVm = _httpContext.HttpContext.Session.Get<UserViewModel>("user");
            return userVm == null ? false : true;
        }
    }
}
