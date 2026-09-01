using MediatR;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScope.Query
{
    public class GetAllAssignmentScopeQuery : IRequest<TResult<List<DTO.AssignmentScopeInfo>>>
    {
    }
}
