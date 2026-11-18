using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public sealed class GetStudentFinanceSummaryQueryHandler
        : IRequestHandler<GetStudentFinanceSummaryQuery, TResult<StudentFinanceSummaryInfo>>
    {
        private readonly IStudentInvoiceRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetStudentFinanceSummaryQueryHandler(
            IStudentInvoiceRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<TResult<StudentFinanceSummaryInfo>> Handle(
            GetStudentFinanceSummaryQuery request, CancellationToken cancellationToken)
        {
            Guid studentID;
            if (_currentUser.IsAdmin)
            {
                if (!Guid.TryParse(request.StudentID, out studentID) || studentID == Guid.Empty)
                    return TResult<StudentFinanceSummaryInfo>.Fail("StudentID is required for an administrator.");
            }
            else if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                     _currentUser.EntityGuid is Guid entityID && entityID != Guid.Empty)
            {
                studentID = entityID;
            }
            else
            {
                return TResult<StudentFinanceSummaryInfo>.Fail("Student finance summary is not available for this user.");
            }

            var summary = await _repository.GetStudentFinanceSummary(studentID);
            return summary == null
                ? TResult<StudentFinanceSummaryInfo>.Fail("Student was not found.")
                : TResult<StudentFinanceSummaryInfo>.OK(summary);
        }
    }
}
