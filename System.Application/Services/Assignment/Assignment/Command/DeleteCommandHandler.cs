using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class DeleteAssignmentCommandHandler : IRequestHandler<DeleteAssignmentCommand, TResult<string>>
    {
        private readonly IAssignmentRepository _repo;
        private readonly IAssignmentNominationRepository _nominationRepository;
        private readonly IResultRepository _resultRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAssignmentCommandHandler(
            IAssignmentRepository repo,
            IAssignmentNominationRepository nominationRepository,
            IResultRepository resultRepository,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _nominationRepository = nominationRepository;
            _resultRepository = resultRepository;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.AssignmentID, out var assignmentID) || assignmentID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);
            var entity = await _repo.GetEntityBy(assignmentID);
            if (entity is null)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);

            var nomination = await _nominationRepository.GetEntityByAssigmetID(assignmentID);
            if (nomination is null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);
            if (_currentUserService.UserId is not Guid userID || userID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var results = await _resultRepository.GetListByAssignmentID(assignmentID);
            // Keep the audit trail: soft-delete result rows and cancel the nomination instead of hard-deleting dependents.
            foreach (var result in results)
            {
                result.SetDeleted(userID.ToString());
                await _resultRepository.Update(result);
            }

            nomination.SetNominationState(enAssignmentType.Canceled);
            nomination.SetModified(userID.ToString());
            await _nominationRepository.Update(nomination);

            entity.SetDeleted(userID.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
