using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class UpdateTeachingAssignmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string EnrollmentID { get; set; } = string.Empty;
        public string TeacherID { get; set; } = string.Empty;
        public string GroupID { get; set; } = string.Empty;
        public string SubjectID { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}
