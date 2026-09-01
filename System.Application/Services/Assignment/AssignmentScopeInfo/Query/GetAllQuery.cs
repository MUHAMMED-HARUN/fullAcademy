using MediatR;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Query
{
    public class GetAllAssignmentScopeInfoQuery : IRequest<TResult<List<DTO.AssignmentScopeInfoInfo>>>
    {
    }
}
