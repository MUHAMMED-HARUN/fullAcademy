using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class UpdateTeachingAssignmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string EnrollmentID { get; set; }
        public string TeacherID { get; set; }
        public string GroupID { get; set; }
        public string SubjectID { get; set; }
        public DateOnly Date { get; set; }
    }
}
