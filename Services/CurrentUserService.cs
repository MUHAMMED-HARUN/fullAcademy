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
using System.Domain.Entities.Permissions;
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

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? Role => Roles.FirstOrDefault();

        public List<string> Roles => User?.Claims
            .Where(claim => claim.Type is "Role" or ClaimTypes.Role)
            .Select(claim => claim.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? new List<string>();

        public List<string> Permissions => User?.FindAll("Permission")
            .Select(claim => clsPermission.NormalizePermissionName(claim.Value))
            .Distinct(StringComparer.Ordinal)
            .ToList() ?? new List<string>();

        public bool IsAdmin => Roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);

        public bool HasPermission(string permission) =>
            IsAdmin || Permissions.Contains(permission, StringComparer.Ordinal);

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

        public Guid? EntityGuid =>
            Guid.TryParse(EntityId, out var entityGuid) && entityGuid != Guid.Empty
                ? entityGuid
                : null;
    }
}
