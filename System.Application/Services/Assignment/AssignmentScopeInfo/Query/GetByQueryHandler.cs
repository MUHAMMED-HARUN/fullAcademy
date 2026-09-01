using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Application.Services.Assignment.AssignmentScopeInfo.Query;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Query
{
    public class GetAssignmentScopeInfoByQueryHandler : IRequestHandler<GetAssignmentScopeInfoByQuery, TResult<DTO.AssignmentScopeInfoInfo>>
    {
        private readonly IAssignmentScopeInfoRepository _repo;

        public GetAssignmentScopeInfoByQueryHandler(IAssignmentScopeInfoRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<DTO.AssignmentScopeInfoInfo>> Handle(GetAssignmentScopeInfoByQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetBy(clsUtil.ToGuid(request.AssignmentScopeInfoID));
            if (result == null)
                return TResult<DTO.AssignmentScopeInfoInfo>.Fail(ApplicationExMsg.NotFound);
            
            return TResult<DTO.AssignmentScopeInfoInfo>.OK(result);
        }
    }
}
