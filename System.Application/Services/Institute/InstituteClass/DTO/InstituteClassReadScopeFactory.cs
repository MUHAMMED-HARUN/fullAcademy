using System.Application.Abstractions.Auth;

namespace System.Application.Services.Institute.InstituteClass.DTO
{
    public static class InstituteClassReadScopeFactory
    {
        public static InstituteClassReadScope? Create(ICurrentUserService currentUser)
        {
            if (!currentUser.IsAuthenticated)
                return null;
            if (currentUser.IsAdmin)
                return new InstituteClassReadScope { IsAdmin = true };
            if (!currentUser.HasPermission("Can_Read_InstituteClass"))
                return null;

            if (string.Equals(currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
                return new InstituteClassReadScope { TeacherID = teacherID };
            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
                return new InstituteClassReadScope { StudentID = studentID };

            return null;
        }
    }
}
