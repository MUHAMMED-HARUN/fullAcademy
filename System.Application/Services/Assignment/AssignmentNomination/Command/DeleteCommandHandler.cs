using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.AssignmentNomination.Command;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class DeleteAssignmentNominationCommandHandler : IRequestHandler<DeleteAssignmentNominationCommand, TResult<string>>
    {
        private readonly IAssignmentNominationRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAssignmentNominationCommandHandler(IAssignmentNominationRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteAssignmentNominationCommand request, CancellationToken cancellationToken)
        {
            clsAssignmentNomination entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.AssignmentNominationID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
