using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Finance.StudentInvoice.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public sealed class GenerateInvoicesForEnrollmentCommandHandler
        : IRequestHandler<GenerateInvoicesForEnrollmentCommand, TResult<StudentInvoiceInfo>>
    {
        private readonly IStudentInvoiceRepository _invoiceRepository;
        private readonly IStudentAcademicEnrollmentRepository _enrollmentRepository;
        private readonly IFeeStructureRepository _feeStructureRepository;
        private readonly ICurrentUserService _currentUser;

        public GenerateInvoicesForEnrollmentCommandHandler(
            IStudentInvoiceRepository invoiceRepository,
            IStudentAcademicEnrollmentRepository enrollmentRepository,
            IFeeStructureRepository feeStructureRepository,
            ICurrentUserService currentUser)
        {
            _invoiceRepository = invoiceRepository;
            _enrollmentRepository = enrollmentRepository;
            _feeStructureRepository = feeStructureRepository;
            _currentUser = currentUser;
        }

        public async Task<TResult<StudentInvoiceInfo>> Handle(
            GenerateInvoicesForEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty ||
                !Guid.TryParse(request.FeeStructureID, out var feeStructureID) || feeStructureID == Guid.Empty)
                return TResult<StudentInvoiceInfo>.Fail("Enrollment and fee structure IDs must be valid.");

            var periodToday = DateOnly.FromDateTime(DateTime.Today);
            var enrollment = await _enrollmentRepository.GetEntityForInvoice(enrollmentID);
            if (enrollment == null)
                return TResult<StudentInvoiceInfo>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);

            if (enrollment.Status != System.Domain.Enums.Enrollment.enStudentAcademicEnrollmentStatus.Active ||
                request.BillingPeriodStart < enrollment.ValidFrom ||
                (enrollment.ValidTo.HasValue && request.BillingPeriodEnd > enrollment.ValidTo.Value))
                return TResult<StudentInvoiceInfo>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);

            if (enrollment.AcademicPlan.Version != enrollment.AcademicPlanVersion)
                return TResult<StudentInvoiceInfo>.Fail("The academic plan version no longer matches the enrollment snapshot.");

            var fee = await _feeStructureRepository.GetBy(
                feeStructureID.ToString(), enFeeStructureParamType.ID);
            if (fee == null || clsUtil.ToGuid( fee.AcademicPlanID )!= enrollment.AcademicPlanID ||
                fee.AcademicPlanVersion != enrollment.AcademicPlanVersion)
                return TResult<StudentInvoiceInfo>.Fail("The selected fee structure does not match the enrolled academic plan version.");

            if (await _invoiceRepository.GetGeneratedInvoice(
                    enrollmentID, feeStructureID, request.BillingPeriodStart, request.BillingPeriodEnd) != null)
                return TResult<StudentInvoiceInfo>.Fail("An invoice already exists for this enrollment, fee structure, and billing period.");

            var invoice = new clsStudentInvoice(
                enrollment.StudentID,
                feeStructureID,
                periodToday,
                fee.Amount,
                0,
                request.Notes);
            invoice.SetEnrollmentSnapshot(
                enrollmentID,
                enrollment.AcademicPlanVersion,
                fee.AcademicPlanName,
                fee.Name,
                request.BillingPeriodStart,
                request.BillingPeriodEnd);
            invoice.SetCreated(_currentUser.UserId?.ToString() ?? "System");

            var invoiceID = await _invoiceRepository.Add(invoice);
            if (!Guid.TryParse(invoiceID, out var id) || id == Guid.Empty)
                return TResult<StudentInvoiceInfo>.Fail(ApplicationExMsg.FailAdd);

            var studentName = string.Join(" ", new[]
            {
                enrollment.Student.Person.FirstName,
                enrollment.Student.Person.FatherName,
                enrollment.Student.Person.LastName,
            }.Where(part => !string.IsNullOrWhiteSpace(part)));

            return TResult<StudentInvoiceInfo>.OK(new StudentInvoiceInfo
            {
                StudentInvoiceID = id.ToString(),
                StudentID = enrollment.StudentID.ToString(),
                StudentName = studentName,
                StudentAcademicEnrollmentID = enrollmentID.ToString(),
                AcademicPlanID = fee.AcademicPlanID,
                AcademicPlanName = fee.AcademicPlanName,
                AcademicPlanVersion = enrollment.AcademicPlanVersion,
                FeeStructureID = feeStructureID.ToString(),
                FeeStructureName = fee.Name,
                BillingPeriodStart = request.BillingPeriodStart,
                BillingPeriodEnd = request.BillingPeriodEnd,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                OriginalAmount = invoice.OriginalAmount,
                DiscountAmount = invoice.DiscountAmount,
                PaidAmount = 0,
                RemainingAmount = invoice.RemainingAmount,
                Status = invoice.Status,
                Notes = invoice.Notes,
                HasPayments = false,
            });
        }
    }
}
