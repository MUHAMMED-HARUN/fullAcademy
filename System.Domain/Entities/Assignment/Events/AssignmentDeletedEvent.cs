
using System.Domain.Enums.Enrollment;
using System.Domain.Events.Base;


public record AssigmentDeletedEvent(Guid AssigmentID, Guid AssignmentNominationID,enAssignmentType AssignmentState) : IDomainEvent;
