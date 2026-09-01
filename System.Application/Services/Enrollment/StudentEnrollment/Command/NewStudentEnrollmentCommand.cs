using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class NewStudentEnrollmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentID { get; set; }
        public string GroupID { get; set; }
        public DateOnly Date { get; set; }
    }
}
