
using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Authentication.AuthDTO;

namespace System.Application.Services.Authentication.RolePermission.Command
{
    public class RolePermissionChangeDto
    {
        public string RoleId { get; set; } = string.Empty; 
        public string PermissionName { get; set; } = string.Empty; 
        public bool IsDelete { get; set; } // true = حذف ، false = إضافة
    }
    public class UpdateRolePermissionsCommand: ITransaction, IRequest<TResult<bool>>
    {
        public List<RolePermissionChangeDto> Changes { get; set; } = new List<RolePermissionChangeDto>(); 
    }
}
