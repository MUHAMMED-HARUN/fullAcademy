using System.Security.Claims;

namespace System.Application.Abstractions.Auth
{
    public interface ICurrentUserService
    {
        string? UserID { get; }
        Guid? UserId { get; }
        string? UserName { get; }
        string? Role { get; }
        List<string> Roles { get; }
        List<string> Permissions { get; }
        bool IsAdmin { get; }
        bool HasPermission(string permission);
        string? AccessToken { get; }
        bool IsAuthenticated { get; }

        // ✅ خصائص جديدة (اختيارية)
        string? EntityId { get; }      // معرف الكيان (Person/Teacher/Student)
        Guid? EntityGuid { get; }
        string? EntityType { get; }    // نوع الكيان (Admin/Teacher/Student)
    }
}
