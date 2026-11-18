using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class NewStudentEnrollmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string GroupID { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}
