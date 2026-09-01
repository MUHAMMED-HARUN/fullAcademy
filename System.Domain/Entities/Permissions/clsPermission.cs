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
    static  public List<PermissionStruct> UserPermission()
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
    }
}

