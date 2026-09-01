using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;

namespace System.Application.Services.Enrollment.StudentEnrollment.Query
{
    public class IsExistStudentEnrollmentByQueryHandler : IRequestHandler<IsExistStudentEnrollmentByQuery, TResult<bool>>
    {
        private readonly IStudentEnrollmentRepository _repo;

        public IsExistStudentEnrollmentByQueryHandler(IStudentEnrollmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<bool>> Handle(IsExistStudentEnrollmentByQuery request, CancellationToken cancellationToken)
        {
            return TResult<bool>.OK(await _repo.IsExistBy(request.Param, request.ParamType));
        }
    }
}
