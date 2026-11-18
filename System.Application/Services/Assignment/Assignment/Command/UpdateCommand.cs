using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class UpdateAssignmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentID { get; set; } = string.Empty;
        public string ScheduledDate { get; set; } = string.Empty;
    }
}
