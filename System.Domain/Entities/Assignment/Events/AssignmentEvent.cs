
using System.Domain.Events.Base;


public record AssigmentCreatedEvent(Guid AssigmentID, Guid AssignmentNominationID) : IDomainEvent;
