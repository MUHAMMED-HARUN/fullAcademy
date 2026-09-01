using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Query
{
    public class GetAllAssignmentScopeInfoQueryHandler : IRequestHandler<GetAllAssignmentScopeInfoQuery, TResult<List<DTO.AssignmentScopeInfoInfo>>>
    {
        private readonly IAssignmentScopeInfoRepository _repo;

        public GetAllAssignmentScopeInfoQueryHandler(IAssignmentScopeInfoRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<DTO.AssignmentScopeInfoInfo>>> Handle(GetAllAssignmentScopeInfoQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetList();
            if (result == null || result.Count == 0)
                return TResult<List<DTO.AssignmentScopeInfoInfo>>.Fail(ApplicationExMsg.NotFound);

            return TResult<List<DTO.AssignmentScopeInfoInfo>>.OK(result);
        }
    }
}
