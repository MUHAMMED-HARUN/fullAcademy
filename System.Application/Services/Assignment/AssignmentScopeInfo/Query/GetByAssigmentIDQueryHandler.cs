using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
namespace System.Application.Services.Assignment.AssignmentScopeInfo.Query
{
    public class GetByAssigmentIDQueryHandler : IRequestHandler<GetByAssigmentIDQuery, TResult<List<DTO.AssignmentScopeInfoInfo>>>
    {
        private readonly IAssignmentScopeInfoRepository _repo;

        public GetByAssigmentIDQueryHandler(IAssignmentScopeInfoRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<DTO.AssignmentScopeInfoInfo>>> Handle(GetByAssigmentIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetListForAssigmet(clsUtil.ToGuid(request.AssignmentID));
            if (result == null)
                return TResult<List<DTO.AssignmentScopeInfoInfo>>.Fail(ApplicationExMsg.NotFound);
            
            return TResult<List<DTO.AssignmentScopeInfoInfo>>.OK(result);
        }
    }
}
