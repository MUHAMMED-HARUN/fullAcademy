
using System.Domain.Events.Base;


public record ResultCreatedEvent(Guid ResultID, Guid AssigmentID, Guid AssignmentScopeInfoID,double Score) : IDomainEvent;
