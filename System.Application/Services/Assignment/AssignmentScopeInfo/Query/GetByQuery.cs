using MediatR;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Query
{
    public class GetAssignmentScopeInfoByQuery : IRequest<TResult<DTO.AssignmentScopeInfoInfo>>
    {
        public string AssignmentScopeInfoID { get; set; }
    }
}
