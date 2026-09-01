using MediatR;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScope.DTO;

namespace System.Application.Services.Assignment.AssignmentScope.Query
{
    public class GetAssignmentScopeByQuery : IRequest<TResult<DTO.AssignmentScopeInfo>>
    {
        public string ID { get; set; }
    }
}
