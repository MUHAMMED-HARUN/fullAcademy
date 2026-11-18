using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.Assignment.DTO;
using Gridify;

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

    public sealed class GetAssignmentForUpsertQueryHandler : IRequestHandler<GetAssignmentForUpsertQuery, TResult<AssignmentInfo>>
    {
        private readonly IAssignmentRepository _repo;

        public GetAssignmentForUpsertQueryHandler(IAssignmentRepository repo) => _repo = repo;

        public async Task<TResult<AssignmentInfo>> Handle(GetAssignmentForUpsertQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.AssignmentID, out var assignmentID) || assignmentID == Guid.Empty)
                return TResult<AssignmentInfo>.Fail(ApplicationExMsg.NotFound);

            var result = await _repo.GetForUpsert(assignmentID);
            return result is null
                ? TResult<AssignmentInfo>.Fail(ApplicationExMsg.NotFound)
                : TResult<AssignmentInfo>.OK(result);
        }
    }

    public class GetAllAssignmentQueryHandler : IRequestHandler<GetAllAssignmentQuery, TResult<Paging<AssignmentInfo>>>
    {
        private readonly IAssignmentRepository _repo;

        public GetAllAssignmentQueryHandler(IAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<AssignmentInfo>>> Handle(GetAllAssignmentQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<AssignmentInfo>>.OK(await _repo.GetListFilter(request));
        }
    }
}
