using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.AssignmentScope.Command;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.AssignmentScope.Command
{
    public class DeleteAssignmentScopeCommandHandler : IRequestHandler<DeleteAssignmentScopeCommand, TResult<string>>
    {
        private readonly IAssignmentScopeRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAssignmentScopeCommandHandler(IAssignmentScopeRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteAssignmentScopeCommand request, CancellationToken cancellationToken)
        {
            clsAssignmentScope entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.AssignmentScopeID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
