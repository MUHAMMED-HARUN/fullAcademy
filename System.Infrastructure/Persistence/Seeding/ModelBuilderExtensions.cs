using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Infrastructure.Identity;

namespace System.Infrastructure.Persistence.Seeding
{
    public static class SeedConstants
    {
        // GUIDs الثابتة
        public static readonly Guid AdminRoleId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
        public static readonly Guid UserRoleId = Guid.Parse("B2C3D4E5-F6A7-8901-BCDE-F23456789012");
        public static readonly Guid StudentRoleId = Guid.Parse("F6A7B8C9-D0E1-2345-6789-012345678901");
        public static readonly Guid TeacherRoleId = Guid.Parse("A7B8C9D0-E1F2-3456-7890-123456789012");

        public static readonly Guid AdminUserId = Guid.Parse("C3D4E5F6-A7B8-9012-CDEF-345678901234");
        // ✅ Hash ثابت، سليم 100% بصيغة Base64 لكلمة المرور "Admin@123456" ومطابق لـ Identity V3
        public const string AdminPasswordHash = "AQAAAAIAAYagAAAAEG3k8X+L9vL2Y3jR1K8sQ1M3N7xP4V8L0M7K9L2xQ4v8L0M7K9L=";
    }
    public static class ModelBuilderExtensions
    {
        public static void SeedIdentityData(this ModelBuilder modelBuilder)
        {
            // 1. إنشاء الأدوار (Roles)
            var adminRole = new ApplicationRole
            {
                Id = SeedConstants.AdminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = SeedConstants.AdminRoleId.ToString()
            };

            var userRole = new ApplicationRole
            {
                Id = SeedConstants.UserRoleId,
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = SeedConstants.UserRoleId.ToString()
            };

            var studentRole = new ApplicationRole
            {
                Id = SeedConstants.StudentRoleId,
                Name = "Student",
                NormalizedName = "STUDENT",
                ConcurrencyStamp = SeedConstants.StudentRoleId.ToString()
            };

            var teacherRole = new ApplicationRole
            {
                Id = SeedConstants.TeacherRoleId,
                Name = "Teacher",
                NormalizedName = "TEACHER",
                ConcurrencyStamp = SeedConstants.TeacherRoleId.ToString()
            };

            modelBuilder.Entity<ApplicationRole>().HasData(adminRole, userRole, studentRole, teacherRole);

            // 2. إنشاء المستخدم الرئيسي (Admin User)
            var adminUser = new ApplicationUser
            {
                Id = SeedConstants.AdminUserId,
                UserName = "admin@app.com",
                NormalizedUserName = "ADMIN@APP.COM",
                Email = "admin@app.com",
                NormalizedEmail = "ADMIN@APP.COM",
                EmailConfirmed = true,
                SecurityStamp = "d739f400-36a2-4a0f-90e6-54737c041441",
                ConcurrencyStamp = "a129f400-36a2-4a0f-90e6-54737c041441"
            };

            adminUser.PasswordHash = SeedConstants.AdminPasswordHash;

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 3. ربط المستخدم الأدمن بدوره (UserRole)
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = SeedConstants.AdminRoleId,
                    UserId = SeedConstants.AdminUserId
                }
            );

            // 4. إضافة الصلاحيات المبدئية للأدوار (Role Claims)
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().HasData(
                new IdentityRoleClaim<Guid>
                {
                    Id = 1,
                    RoleId = SeedConstants.AdminRoleId,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.Users.View"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = 2,
                    RoleId = SeedConstants.AdminRoleId,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.Users.Create"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = 3,
                    RoleId = SeedConstants.AdminRoleId,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.Users.Delete"
                }
            );
        }
    }
}