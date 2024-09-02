using Common.Collection.Enums;
using System.Security.Claims;

namespace WebApi.Helpers
{
    public interface IHttpContextHelper
    {
        string GetUserId();
        UserRoles GetUserRole();
    }

    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            if (_httpContextAccessor.HttpContext?.User == null) return string.Empty;
            return _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value.ToString() ?? string.Empty;
        }

        public UserRoles GetUserRole()
        {
            var userRoleValue = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (userRoleValue is null)
            {
                return UserRoles.Error;
            }
            return (UserRoles)System.Enum.Parse(typeof(UserRoles), userRoleValue);
        }
    }
}
