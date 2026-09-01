using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class GetTeachingAssignmentByQueryHandler : IRequestHandler<GetTeachingAssignmentByQuery, TResult<TeachingAssignmentInfo>>
    {
        private readonly ITeachingAssignmentRepository _repo;

        public GetTeachingAssignmentByQueryHandler(ITeachingAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<TeachingAssignmentInfo>> Handle(GetTeachingAssignmentByQuery request, CancellationToken cancellationToken)
        {
            TeachingAssignmentInfo info = await _repo.GetBy(request.Param, request.ParamType);
            if (info == null)
                return TResult<TeachingAssignmentInfo>.Fail(ApplicationExMsg.TeachingAssignmentNotFound);

            return TResult<TeachingAssignmentInfo>.OK(info);
        }
    }
}
