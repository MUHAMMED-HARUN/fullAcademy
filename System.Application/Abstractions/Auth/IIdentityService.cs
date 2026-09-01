
using System.Application.Common;
using System.Application.Services.Authentication.Login.DTO;
using System.Application.Services.Authentication.Role.DTO;
using System.Application.Services.Authentication.RolePermission.Command;
using System.Application.Services.Authentication.RolePermission.DTO;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Auth
{
    public interface IIdentityService
    {
        Task<TResult<string>> RegisterUserAsync(string email, string password, string userName,enUserType userType,string entityID);// return UserId as String
        
        Task<TResult<LoginDto>> LoginAsync(string email, string password);// return Token as string
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<bool> AuthorizeAsync(Guid userId, string policyName);
        Task<List<RoleInfo>> getRoles();
        Task<List<RolePermissionsInfo>> GetRolePermissions(string RoleID);
        Task<bool> DeleteClaims(RolePermissionChangeDto toDelete);
        Task<bool> AddClaims(RolePermissionChangeDto toAdd);
        Task<bool> SetUserRole(string  roleID, string userId);
    }
}
