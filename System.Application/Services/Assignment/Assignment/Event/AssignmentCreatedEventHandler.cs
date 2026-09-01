using MediatR;
using System.Application.Abstractions.Assignment;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.EventHandlers
{
    public class AssignmentCreatedEventHandler : INotificationHandler<AssigmentCreatedEvent>
    {
        private readonly IAssignmentNominationRepository _nominationRepo;

        public AssignmentCreatedEventHandler(IAssignmentNominationRepository nominationRepo)
        {
            _nominationRepo = nominationRepo;
        }

        public async Task Handle(AssigmentCreatedEvent notification, CancellationToken cancellationToken)
        {
            var nomination = await _nominationRepo.GetEntityBy(notification.AssignmentNominationID);

            if (nomination != null)
            {
                nomination.SetNominationState(enAssignmentType.Completed);
                await _nominationRepo.Update(nomination);
            }
        }
    }
}