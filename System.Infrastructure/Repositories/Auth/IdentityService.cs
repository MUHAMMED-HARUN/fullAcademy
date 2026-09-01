
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Authentication.Login.DTO;
using System.Application.Services.Authentication.Role.DTO;
using System.Application.Services.Authentication.RolePermission.Command;
using System.Application.Services.Authentication.RolePermission.DTO;
using System.Domain.Enums.Person;
using System.Infrastructure.Identity;
using System.Security.Claims;

namespace System.Infrastructure.Repositories.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _tokenGenerator; // واجهة سنصنعها لتوليد التوكن
        private readonly RoleManager<ApplicationRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager1, IJwtTokenGenerator tokenGenerator,RoleManager<ApplicationRole> manager)
        {
            _userManager = userManager1;
            _tokenGenerator = tokenGenerator;
            _roleManager = manager;
        }

        public Task<bool> AuthorizeAsync(Guid userId, string policyName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<TResult<LoginDto>> LoginAsync(string email, string password)
        {
          var user=await  _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("Email Not Found");

            var isValidPass= await _userManager.CheckPasswordAsync(user, password);

            if (!isValidPass)
                throw new Exception("Password Not Seccuess");
            var role=await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();

            foreach (var r in role)
            {
                var rollRes=await _roleManager.FindByNameAsync(r);
                var clims=await _roleManager.GetClaimsAsync(rollRes);
                permissions.AddRange(clims.Where(c => c.Type == "Permission").Select(c => c.Value));

            }
        permissions=    permissions.Distinct().ToList();
            var token=_tokenGenerator.GenerateToken(user, role,permissions);


            var loginDto = new LoginDto
            {
                ID = user.Id.ToString(),
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
            
            return TResult<LoginDto>.OK(loginDto);
        }

        public async Task<TResult<string>> RegisterUserAsync(string email, string password, string userName, enUserType userType, string entityID)
        {

            ApplicationUser user = new ApplicationUser();
            user.Email = email;
            user.UserName = userName;
            user.EntytyType = userType;
            user.EntytyID = clsUtil.ToGuid(entityID);
            
            var result = await _userManager.CreateAsync(user,password);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return TResult<string>.Fail(new ApplicationExceptionObj("create_User_Error", errorMessage, "infrastructur"));
            }

            // يمكن إضافة الـ Default Role هنا إذا لزم الأمر
            // await _userManager.AddToRoleAsync(user, "User");

            return TResult<string>.OK(user.Id.ToString());
        }
        public  async Task<List<RoleInfo>> getRoles()
        {
            return  await _roleManager.Roles.Select(r=>new RoleInfo { ID=r.Id.ToString(),Name=r.Name}).ToListAsync();
        }

        public async Task<List<RolePermissionsInfo>> GetRolePermissions(string RoleID)
        {
            var Role= await  _roleManager.Roles.Where(r => r.Id == clsUtil.ToGuid(RoleID)).FirstOrDefaultAsync();
            if (Role == null)
                return null;
            var Clims = await _roleManager.GetClaimsAsync(Role);

          var RoleClims=  Clims.Select(c => new RolePermissionsInfo { RoleId=Role.Id.ToString(),RoleName=Role.Name,
                PermissionName=c.Value }).ToList();
            return RoleClims;
        }

        public async Task<bool> DeleteClaims(RolePermissionChangeDto toDelete)
        {
            // 1. البحث عن الدور بواسطة الـ RoleId
            var role = await _roleManager.FindByIdAsync(toDelete.RoleId);
            if (role == null) return false;

            // 2. جلب الـ Claims المسجلة للدور حالياً
            var existingClaims = await _roleManager.GetClaimsAsync(role);

            // 3. البحث عن الـ Claim المطلوب حسابه
            var claimToRemove = existingClaims.FirstOrDefault(c =>
                c.Type == "Permission" && c.Value == toDelete.PermissionName);

            // إذا كانت الصلاحية غير موجودة بالأصل نرجع true (لأن الهدف محقق)
            if (claimToRemove == null) return true;

            // 4. حذف الـ Claim
            var result = await _roleManager.RemoveClaimAsync(role, claimToRemove);
            return result.Succeeded;
        }

        public async Task<bool> AddClaims(RolePermissionChangeDto toAdd)
        {
            // 1. البحث عن الدور بواسطة الـ RoleId
            var role = await _roleManager.FindByIdAsync(toAdd.RoleId);
            if (role == null) return false;

            // 2. جلب الـ Claims المسجلة للدور لضمان عدم التكرار
            var existingClaims = await _roleManager.GetClaimsAsync(role);

            var exists = existingClaims.Any(c =>
                c.Type == "Permission" && c.Value == toAdd.PermissionName);

            // إذا كانت الصلاحية موجودة بالفعل لا داعي لإعادتها
            if (exists) return true;

            // 3. إنشاء الـ Claim الجديد وإضافته
            var claimToAdd = new Claim("Permission", toAdd.PermissionName);
            var result = await _roleManager.AddClaimAsync(role, claimToAdd);

            return result.Succeeded;
        }

        public async Task<bool> SetUserRole(string roleID, string userId)
        {
            var role = await _roleManager.Roles.Where(r => r.Id == clsUtil.ToGuid(roleID)).FirstOrDefaultAsync();
            var user = await _userManager.Users.Where(u => u.Id == clsUtil.ToGuid(userId)).FirstOrDefaultAsync();
            if (role == null || user == null)
                return false;
         var res= await  _userManager.AddToRoleAsync(user, role.Name);
            return res.Succeeded;
        }
    }
}
