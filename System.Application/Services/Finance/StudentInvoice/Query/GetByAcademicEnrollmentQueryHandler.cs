using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public sealed class GetByAcademicEnrollmentQueryHandler
        : IRequestHandler<GetByAcademicEnrollmentQuery, TResult<List<StudentInvoiceInfo>>>
    {
        private readonly IStudentInvoiceRepository _repository;
        public GetByAcademicEnrollmentQueryHandler(IStudentInvoiceRepository repository) => _repository = repository;

        public async Task<TResult<List<StudentInvoiceInfo>>> Handle(
            GetByAcademicEnrollmentQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty)
                return TResult<List<StudentInvoiceInfo>>.Fail("StudentAcademicEnrollmentID must be a valid identifier.");

            return TResult<List<StudentInvoiceInfo>>.OK(await _repository.GetByAcademicEnrollment(enrollmentID));
        }
    }
}
