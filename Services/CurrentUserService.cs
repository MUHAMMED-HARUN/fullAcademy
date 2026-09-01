//using System.Security.Claims;
//using System.Application.Abstractions.Auth;
//using System.Application.Common;

//namespace School_Academy.Services
//{
//    public class CurrentUserService : ICurrentUserService
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public string UserID
//        {
//            get
//            {
//                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
//                // Return a dummy GUID for now if null/empty, or Guid.Empty, as we might not have auth setup yet.
//                // Assuming Guid.Empty is safe or handled.
//                return  userId!=null?userId : "9a3c4bac-341a-43b7-ed1c-08deea872f13"; // Using NewGuid for testing if no auth
//            }

//        }

//        public Guid? UserId => clsUtil.ToGuid(UserID);

//        public string UserName
//        {
//            get
//            {
//                var username = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
//                // Return a dummy GUID for now if null/empty, or Guid.Empty, as we might not have auth setup yet.
//                // Assuming Guid.Empty is safe or handled.
//                return username; // Using NewGuid for testing if no auth
//            }
//        }

//        public bool IsAuthenticated => throw new NotImplementedException();
//    }
//}using System.Security.Claims;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Security.Claims;

namespace School_Academy.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserID => _httpContextAccessor.HttpContext?.User?.FindFirstValue("UserId");
        public Guid? UserId => clsUtil.ToGuid(UserID);

        public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("UserName");

        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue("Role");
        public List<string> Roles => _httpContextAccessor.HttpContext?.User?.FindAll("Role")
                                                      .Select(c => c.Value).ToList();

        public string? AccessToken
        {
            get
            {
                var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return authHeader["Bearer ".Length..].Trim();
                }
                return null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? EntityId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("EntityId");

        public string? EntityType => _httpContextAccessor.HttpContext?.User?.FindFirstValue("EntityType");
    }
}