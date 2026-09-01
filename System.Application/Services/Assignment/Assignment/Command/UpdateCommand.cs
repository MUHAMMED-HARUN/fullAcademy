using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class UpdateAssignmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentID { get; set; }    
        public string AssignmentNominationID { get; set; }
        public string CommitteeID { get; set; }
        public enCommitteeType CommitteeType { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public enAssignmentType AssignmentState { get; set; }
        public bool? IsPassed { get; set; }
    }
}
