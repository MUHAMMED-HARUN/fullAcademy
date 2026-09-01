using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class IsExistTeachingAssignmentByQueryHandler : IRequestHandler<IsExistTeachingAssignmentByQuery, TResult<bool>>
    {
        private readonly ITeachingAssignmentRepository _repo;

        public IsExistTeachingAssignmentByQueryHandler(ITeachingAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<bool>> Handle(IsExistTeachingAssignmentByQuery request, CancellationToken cancellationToken)
        {
            return TResult<bool>.OK(await _repo.IsExistBy(request.Param, request.ParamType));
        }
    }
}
