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
    public class GetAllPermission :IRequest<TResult<List<PermissionStruct>>>,IQuery
    {
    }
}
