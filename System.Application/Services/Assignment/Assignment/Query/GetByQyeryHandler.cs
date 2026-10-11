using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.Assignment.DTO;

namespace System.Application.Services.Assignment.Assignment.Query
{
    public class GetAssignmentByQueryHandler : IRequestHandler<GetAssignmentByQuery, TResult<AssignmentInfo>>
    {
        private readonly IAssignmentRepository _repo;

        public GetAssignmentByQueryHandler(IAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<AssignmentInfo>> Handle(GetAssignmentByQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetBy(clsUtil.ToGuid(request.ID));
            if (result == null)
                return TResult<AssignmentInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<AssignmentInfo>.OK(result);
        }
    }

    public class GetAllAssignmentQueryHandler : IRequestHandler<GetAllAssignmentQuery, TResult<List<AssignmentInfo>>>
    {
        private readonly IAssignmentRepository _repo;

        public GetAllAssignmentQueryHandler(IAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<AssignmentInfo>>> Handle(GetAllAssignmentQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetList();
            if (result == null || result.Count == 0)
                return TResult<List<AssignmentInfo>>.Fail(ApplicationExMsg.NotFound);

            return TResult<List<AssignmentInfo>>.OK(result);
        }
    }
}
