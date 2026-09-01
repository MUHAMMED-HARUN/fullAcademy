using MediatR;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScope.Query
{
    public class GetCanNominateQuery : IRequest<TResult<List<DTO.AssignmentScopeInfo>>>
    {
        public string studentEnrollmentID { get; set; }
    }
}
