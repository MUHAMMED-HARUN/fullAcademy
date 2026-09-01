using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class DeleteAssignmentCommandHandler : IRequestHandler<DeleteAssignmentCommand, TResult<string>>
    {
        private readonly IAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAssignmentCommandHandler(IAssignmentRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            clsAssignment entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.AssignmentID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
