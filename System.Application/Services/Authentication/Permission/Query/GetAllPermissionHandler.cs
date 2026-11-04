using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;
using System.Collections.Generic;
using System.Domain.Entities.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Authentication.Permission.Query
{
    public class GetAllPermissionHandler : IRequestHandler<GetAllPermission, TResult<List<PermissionStruct>>>
    {
        public async Task<TResult<List<PermissionStruct>>> Handle(GetAllPermission request, CancellationToken cancellationToken)
        {
            var res = clsPermission.AllPermissions();
            return  TResult<List<PermissionStruct>>.OK(res);
        }
    }
}
