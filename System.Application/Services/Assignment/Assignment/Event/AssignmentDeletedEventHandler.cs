using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.EventHandlers
{
    public class AssignmentDeletedEventHandler : INotificationHandler<AssigmentDeletedEvent>
    {
        private readonly IAssignmentNominationRepository _nominationRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IResultRepository _resultRepo;
        private readonly ICurrentUserService _currentUser;
        public AssignmentDeletedEventHandler(IAssignmentNominationRepository nominationRepo, IAssignmentRepository assignmentRepo, IResultRepository resultRepo, ICurrentUserService currentUser)
        {
            _nominationRepo = nominationRepo;
            _assignmentRepo = assignmentRepo;
            _resultRepo = resultRepo;   
            _currentUser= currentUser;
        }

        public async Task Handle(AssigmentDeletedEvent notification, CancellationToken cancellationToken)
        {
            
            var nomination = await _nominationRepo.GetEntityBy(notification.AssignmentNominationID);
            var result = await _resultRepo.GetListByAssignmentID(  notification.AssigmentID);

            if (nomination != null)
            {
                nomination.SetNominationState(enAssignmentType.Canceled);
            }
            if (result != null && result.Any()) 
                result.ForEach(r => r.SetDeleted(_currentUser.UserID));
        }
    }
}