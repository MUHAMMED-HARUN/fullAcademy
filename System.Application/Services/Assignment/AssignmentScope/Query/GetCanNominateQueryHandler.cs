using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Application.Services.Assignment.AssignmentScopeInfo.DTO;

namespace System.Application.Services.Assignment.AssignmentScope.Query
{
    public class GetCanNominateQueryHandler : IRequestHandler<GetCanNominateQuery, TResult<List<DTO.AssignmentScopeInfo>>>
    {
        private readonly IAssignmentScopeRepository _repo;

        public GetCanNominateQueryHandler(IAssignmentScopeRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<DTO.AssignmentScopeInfo>>> Handle(GetCanNominateQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetCanNominate(request.studentEnrollmentID);
            if (result == null || result.Count == 0)
                return TResult<List<DTO.AssignmentScopeInfo>>.Fail(ApplicationExMsg.NotFound);

            return TResult<List<DTO.AssignmentScopeInfo>>.OK(result);
        }
    }
}
