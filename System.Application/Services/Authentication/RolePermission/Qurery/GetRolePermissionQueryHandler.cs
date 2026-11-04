

using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Authentication.RolePermission.DTO;
using System.Security.Principal;

namespace System.Application.Services.Authentication.RolePermission.Qurery
{
    public class GetRolePermissionQueryHandler : IRequestHandler<GetRolePermissionQuery, TResult<List<RolePermissionsInfo>>>
    {
        IIdentityService _identityService;
        public GetRolePermissionQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<TResult<List<RolePermissionsInfo>>> Handle(GetRolePermissionQuery request, CancellationToken cancellationToken)
        {
            var rolePermissions = await _identityService.GetRolePermissions(request.RoleID);
            if (rolePermissions is not null)
                return TResult<List<RolePermissionsInfo>>.OK(rolePermissions);

            return TResult<List<RolePermissionsInfo>>.Fail("الدور غير موجود");
        }
    }
}
