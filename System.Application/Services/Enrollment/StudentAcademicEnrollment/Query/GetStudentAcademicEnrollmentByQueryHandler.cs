using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Query;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Query
{
    public sealed class GetStudentAcademicEnrollmentByQueryHandler : IRequestHandler<GetStudentAcademicEnrollmentByQuery, TResult<StudentAcademicEnrollmentInfo>>
    {
        private readonly IStudentAcademicEnrollmentRepository _repository;

        public GetStudentAcademicEnrollmentByQueryHandler(IStudentAcademicEnrollmentRepository repository) => _repository = repository;

        public async Task<TResult<StudentAcademicEnrollmentInfo>> Handle(GetStudentAcademicEnrollmentByQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var id) || id == Guid.Empty)
                return TResult<StudentAcademicEnrollmentInfo>.Fail("StudentAcademicEnrollmentID must be a valid GUID");
            var enrollment = await _repository.GetBy(id);
            return enrollment == null
                ? TResult<StudentAcademicEnrollmentInfo>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound)
                : TResult<StudentAcademicEnrollmentInfo>.OK(enrollment);
        }
    }
}
