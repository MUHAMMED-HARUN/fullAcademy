

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Authentication.Role.DTO;
using System.Domain.Entities.Permissions;

namespace System.Application.Services.Authentication.Role.Qurery
{
    public class GetRolesQuery:IRequest<TResult<List<RoleInfo>>>,IQuery
    {
      
    }
}
