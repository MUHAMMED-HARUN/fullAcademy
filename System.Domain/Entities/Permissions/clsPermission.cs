using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Domain.Entities.Permissions
{
    public class PermissionStruct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type{ get; set; }
    }
    public class clsPermission
    {
        public static List<PermissionStruct> UserPermission()
        {
            List<PermissionStruct> UserPermission=new List<PermissionStruct>();
            string type = "User";
            UserPermission.Add(new PermissionStruct { Name = "Can_Update_User", Id = 1 ,Type=type});
            UserPermission.Add(new PermissionStruct { Name = "Can_Read_User", Id =2, Type = type });
            UserPermission.Add(new PermissionStruct { Name = "Can_SetPermission_User", Id = 3, Type = type });
            UserPermission.Add(new PermissionStruct { Name = "Can_Register_User", Id = 4, Type = type });
            UserPermission.Add(new PermissionStruct { Name = "Can_Delete_User", Id = 5, Type = type });

            return UserPermission;
        }

        public static List<PermissionStruct> AllPermissions()
        {
            var permissions = UserPermission();
            var nextId = permissions.Max(permission => permission.Id) + 1;

            AddEntityPermissions(permissions, "Country", ref nextId);
            AddEntityPermissions(permissions, "City", ref nextId);
            AddEntityPermissions(permissions, "District", ref nextId);
            AddEntityPermissions(permissions, "Neighborhood", ref nextId);
            AddEntityPermissions(permissions, "Address", ref nextId);
            AddEntityPermissions(permissions, "Institute", ref nextId);
            AddEntityPermissions(permissions, "Class", ref nextId);
            AddEntityPermissions(permissions, "InstituteClass", ref nextId);
            AddEntityPermissions(permissions, "Session", ref nextId);
            AddEntityPermissions(permissions, "Stage", ref nextId);
            AddEntityPermissions(permissions, "StageInfo", ref nextId);
            AddEntityPermissions(permissions, "DailyEvaluation", ref nextId);
            AddEntityPermissions(permissions, "TeachingAssignment", ref nextId);
            AddEntityPermissions(permissions, "StudentEnrollment", ref nextId);
            AddEntityPermissions(permissions, "Program", ref nextId);
            AddEntityPermissions(permissions, "AcademicPlan", ref nextId);
            AddEntityPermissions(permissions, "StudentAcademicEnrollment", ref nextId);
            AddEntityPermissions(permissions, "Group", ref nextId);
            AddEntityPermissions(permissions, "StudentStage", ref nextId);
            AddEntityPermissions(permissions, "AssignmentScope", ref nextId);
            AddEntityPermissions(permissions, "AssignmentScopeInfo", ref nextId);
            AddEntityPermissions(permissions, "AssignmentNomination", ref nextId);
            AddEntityPermissions(permissions, "Assignment", ref nextId);
            AddEntityPermissions(permissions, "Result", ref nextId);
            AddEntityPermissions(permissions, "FeeStructure", ref nextId);
            AddEntityPermissions(permissions, "StudentInvoice", ref nextId);
            AddEntityPermissions(permissions, "Payment", ref nextId);
            AddEntityPermissions(permissions, "Attendance", ref nextId);

            permissions.Add(new PermissionStruct
            {
                Id = nextId++,
                Name = "Can_Enroll_StudentAcademicPlan",
                Type = "StudentAcademicEnrollment",
            });
            permissions.Add(new PermissionStruct
            {
                Id = nextId++,
                Name = "Can_Generate_StudentInvoice",
                Type = "StudentInvoice",
            });
            permissions.Add(new PermissionStruct
            {
                Id = nextId++,
                Name = "Can_Record_Payment",
                Type = "Payment",
            });
            permissions.Add(new PermissionStruct
            {
                Id = nextId++,
                Name = "Can_Reverse_Payment",
                Type = "Payment",
            });
            permissions.Add(new PermissionStruct
            {
                Id = nextId++,
                Name = "Can_FollowUp_StudentFinance",
                Type = "StudentFinance",
            });

            AddEntityPermissions(permissions, "Person", ref nextId);
            AddEntityPermissions(permissions, "Student", ref nextId);
            AddEntityPermissions(permissions, "Teacher", ref nextId);

            return permissions;
        }

        public static List<string> DefaultPermissionsForRole(string? roleName)
        {
            if (!string.Equals(roleName, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(roleName, "Student", StringComparison.OrdinalIgnoreCase))
            {
                return new List<string>();
            }

            var permissions = new List<string>
            {
                "Can_Read_Country",
                "Can_Read_City",
                "Can_Read_District",
                "Can_Read_Neighborhood",
                "Can_Read_Address",
                "Can_Read_Institute",
                "Can_Read_Program",
                "Can_Read_Class",
                "Can_Read_InstituteClass",
                "Can_Read_Session",
                "Can_Read_Stage",
                "Can_Read_StageInfo",
                "Can_Read_DailyEvaluation",
                "Can_Read_AcademicPlan",
                "Can_Read_StudentAcademicEnrollment",
                "Can_Read_StudentEnrollment",
                "Can_Read_Group",
                "Can_Read_StudentStage",
                "Can_Read_AssignmentScope",
                "Can_Read_AssignmentScopeInfo",
                "Can_Read_AssignmentNomination",
                "Can_Read_Assignment",
                "Can_Read_Result",
                "Can_Read_Attendance",
                "Can_Read_Person",
                "Can_Read_Student",
            };

            if (string.Equals(roleName, "Teacher", StringComparison.OrdinalIgnoreCase))
            {
                permissions.Add("Can_Read_TeachingAssignment");
                permissions.Add("Can_Read_Teacher");
                permissions.Add("Can_Create_DailyEvaluation");
                permissions.Add("Can_Update_DailyEvaluation");
                permissions.Add("Can_Create_Attendance");
                permissions.Add("Can_Update_Attendance");
                permissions.Add("Can_Create_AssignmentNomination");
                permissions.Add("Can_Create_Assignment");
                permissions.Add("Can_Update_Assignment");
                permissions.Add("Can_Create_Result");
                permissions.Add("Can_Update_Result");
            }
            else if (string.Equals(roleName, "Student", StringComparison.OrdinalIgnoreCase))
            {
                permissions.Add("Can_Read_StudentInvoice");
                permissions.Add("Can_Read_Payment");
            }

            return permissions;
        }

        private static void AddEntityPermissions(
            List<PermissionStruct> permissions,
            string entity,
            ref int nextId)
        {
            foreach (var action in new[] { "Read", "Create", "Update", "Delete" })
            {
                permissions.Add(new PermissionStruct
                {
                    Id = nextId++,
                    Name = $"Can_{action}_{entity}",
                    Type = entity,
                });
            }
        }

        public static string NormalizePermissionName(string permissionName) => permissionName switch
        {
            "Permissions.Users.View" => "Can_Read_User",
            "Permissions.Users.Create" => "Can_Register_User",
            "Permissions.Users.Update" => "Can_Update_User",
            "Permissions.Users.Delete" => "Can_Delete_User",
            "Permissions.Users.SetPermission" => "Can_SetPermission_User",
            _ => permissionName,
        };
    }
}

