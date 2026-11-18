using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Command
{
    public sealed class EnrollStudentAcademicPlanCommand : ICommand, ITransaction, IRequest<TResult<string>>
    {
        public string StudentID { get; set; } = string.Empty;
        public string AcademicPlanID { get; set; } = string.Empty;
        public DateOnly EnrollmentDate { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly? ValidTo { get; set; }
    }
}
