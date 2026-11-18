using Gridify;
using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Query;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Query
{
    public sealed class GetAllStudentAcademicEnrollmentFilterQueryHandler : IRequestHandler<GetAllStudentAcademicEnrollmentFilterQuery, TResult<Paging<StudentAcademicEnrollmentLookupInfo>>>
    {
        private readonly IStudentAcademicEnrollmentRepository _repository;

        public GetAllStudentAcademicEnrollmentFilterQueryHandler(IStudentAcademicEnrollmentRepository repository) => _repository = repository;

        public async Task<TResult<Paging<StudentAcademicEnrollmentLookupInfo>>> Handle(GetAllStudentAcademicEnrollmentFilterQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.GroupID) &&
                (!Guid.TryParse(request.GroupID, out var groupID) || groupID == Guid.Empty))
                return TResult<Paging<StudentAcademicEnrollmentLookupInfo>>.Fail("GroupID must be a valid GUID");

            return TResult<Paging<StudentAcademicEnrollmentLookupInfo>>.OK(await _repository.GetListFilter(request));
        }
    }
}
