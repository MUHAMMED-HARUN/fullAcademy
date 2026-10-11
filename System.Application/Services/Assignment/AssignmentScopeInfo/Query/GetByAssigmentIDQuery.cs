using MediatR;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Query
{
    public class GetByAssigmentIDQuery : IRequest<TResult<List<DTO.AssignmentScopeInfoInfo>>>
    {
        public string AssignmentID { get; set; }
    }
}
