using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.AssignmentScope.Command;
using System.Application.Services.Assignment.AssignmentScopeInfo.Command;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Command
{
    public class DeleteAssignmentScopeInfoCommandHandler : IRequestHandler<DeleteAssignmentScopeInfoCommand, TResult<string>>
    {
        private readonly IAssignmentScopeInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAssignmentScopeInfoCommandHandler(IAssignmentScopeInfoRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteAssignmentScopeInfoCommand request, CancellationToken cancellationToken)
        {
            clsAssignmentScopeinfo entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.AssignmentScopeInfoID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
