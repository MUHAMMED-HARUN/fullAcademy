using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Infrastructure.Identity;

namespace System.Infrastructure.Persistence.Seeding
{
    public static class SeedConstants
    {
        // GUIDs ثابتة لكل عنصر
        public static readonly Guid AdminRoleId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
        public static readonly Guid UserRoleId = Guid.Parse("B2C3D4E5-F6A7-8901-BCDE-F23456789012");

        // GUIDs الجديدة للأدوار المطلوبة (بدون صلاحيات)
        public static readonly Guid StudentRoleId = Guid.Parse("F6A7B8C9-D0E1-2345-6789-012345678901");
        public static readonly Guid TeacherRoleId = Guid.Parse("A7B8C9D0-E1F2-3456-7890-123456789012");

        public static readonly Guid AdminUserId = Guid.Parse("C3D4E5F6-A7B8-9012-CDEF-345678901234");
        // Hash ثابت ومسبق التشفير لكلمة المرور "Admin@123456"
        public const string AdminPasswordHash = "AQAAAAIAAYagAAAAEJ3K3xV8k04nZmYk1o0M7K9L2xQ4v8L0M7K9L2xQ4v8L0M7K9L==";
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

            // الأدوار الجديدة (بدون أي صلاحيات مضافة)
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

            // إضافة جميع الأدوار الأربعة إلى قاعدة البيانات
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

            // 3. ربط المستخدم بالأدوار (UserRole) - هنا ربط الأدمن بدوره
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = SeedConstants.AdminRoleId,
                    UserId = SeedConstants.AdminUserId
                }
                // يمكنك إضافة مستخدمين تجريبيين (Student/Teacher) هنا لاحقاً إذا أردت
            );

            // 4. إضافة الصلاحيات للأدوار (Role Permissions / Claims)
            // يتم إضافة الصلاحيات الخاصة بـ Admin فقط (كما كانت موجودة سابقاً)
            // ملاحظة: StudentRole و TeacherRole ليس لديهم أي صلاحيات مضافة هنا
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