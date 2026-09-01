using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;

namespace System.Application.Services.Enrollment.StudentEnrollment.Query
{
    public class GetStudentEnrollmentByQueryHandler : IRequestHandler<GetStudentEnrollmentByQuery, TResult<StudentEnrollmentInfo>>
    {
        private readonly IStudentEnrollmentRepository _repo;

        public GetStudentEnrollmentByQueryHandler(IStudentEnrollmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<StudentEnrollmentInfo>> Handle(GetStudentEnrollmentByQuery request, CancellationToken cancellationToken)
        {
            StudentEnrollmentInfo info = await _repo.GetBy(request.Param, request.ParamType);
            if (info == null)
                return TResult<StudentEnrollmentInfo>.Fail(ApplicationExMsg.StudentEnrollmentNotFound);

            return TResult<StudentEnrollmentInfo>.OK(info);
        }
    }
}
