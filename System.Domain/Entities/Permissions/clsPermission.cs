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

            return permissions;
        }

        public static List<string> DefaultPermissionsForRole(string? roleName)
        {
            if (!string.Equals(roleName, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(roleName, "Student", StringComparison.OrdinalIgnoreCase))
            {
                return new List<string>();
            }

            return new List<string>
            {
                "Can_Read_Country",
                "Can_Read_City",
                "Can_Read_District",
                "Can_Read_Neighborhood",
                "Can_Read_Address",
                "Can_Read_Institute",
                "Can_Read_Class",
                "Can_Read_InstituteClass",
                "Can_Read_Session",
                "Can_Read_Stage",
            };
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

