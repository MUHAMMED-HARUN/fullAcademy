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
        string? AccessToken { get; }
        bool IsAuthenticated { get; }

        // ✅ خصائص جديدة (اختيارية)
        string? EntityId { get; }      // معرف الكيان (Person/Teacher/Student)
        string? EntityType { get; }    // نوع الكيان (Admin/Teacher/Student)
    }
}