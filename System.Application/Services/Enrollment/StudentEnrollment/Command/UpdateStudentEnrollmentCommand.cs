using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class UpdateStudentEnrollmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string EnrollmentID { get; set; }
        public string StudentID { get; set; }
        public string GroupID { get; set; }
        public DateOnly Date { get; set; }
    }
}
