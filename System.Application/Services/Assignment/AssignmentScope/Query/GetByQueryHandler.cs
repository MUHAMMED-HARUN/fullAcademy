using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.AssignmentScope.DTO;

namespace System.Application.Services.Assignment.AssignmentScope.Query
{
    public class GetAssignmentScopeByQueryHandler : IRequestHandler<GetAssignmentScopeByQuery, TResult<DTO.AssignmentScopeInfo>>
    {
        private readonly IAssignmentScopeRepository _repo;

        public GetAssignmentScopeByQueryHandler(IAssignmentScopeRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<DTO.AssignmentScopeInfo>> Handle(GetAssignmentScopeByQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetBy(clsUtil.ToGuid(request.ID));
            if (result == null)
                return TResult<DTO.AssignmentScopeInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<DTO.AssignmentScopeInfo>.OK(result);
        }
    }
}
