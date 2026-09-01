using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class DeleteTeachingAssignmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string TeachingAssignmentID { get; set; }
    }
}
