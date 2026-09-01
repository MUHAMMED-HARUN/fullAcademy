using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class DeleteTeachingAssignmentCommandHandler : IRequestHandler<DeleteTeachingAssignmentCommand, TResult<string>>
    {
        private readonly ITeachingAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTeachingAssignmentCommandHandler(ITeachingAssignmentRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteTeachingAssignmentCommand request, CancellationToken cancellationToken)
        {
            clsTeachingAssignment entity = await _repo.GetEntityBy(request.TeachingAssignmentID, enTeachingAssignmentType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.TeachingAssignmentNotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
