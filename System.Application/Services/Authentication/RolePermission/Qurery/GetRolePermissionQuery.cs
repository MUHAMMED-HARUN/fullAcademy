

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Authentication.RolePermission.DTO;

namespace System.Application.Services.Authentication.RolePermission.Qurery
{
    public class GetRolePermissionQuery:IRequest<TResult<List<RolePermissionsInfo>>>,IQuery
    {
        public string RoleID { get; set; }
    }
}
