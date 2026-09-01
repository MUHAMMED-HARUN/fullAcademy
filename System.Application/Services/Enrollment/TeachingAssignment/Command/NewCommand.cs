using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class NewTeachingAssignmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string TeacherID { get; set; }
        public string GroupID { get; set; }
        public string SubjectID { get; set; }
        public DateOnly Date { get; set; }
    }
}
