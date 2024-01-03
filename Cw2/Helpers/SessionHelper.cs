using Microsoft.AspNetCore.Http;
namespace Cw2.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoggedInUserEmail()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("email");
        }
    }
}
