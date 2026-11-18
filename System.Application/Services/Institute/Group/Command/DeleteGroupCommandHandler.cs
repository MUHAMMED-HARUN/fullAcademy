using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Group.Command
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, TResult<string>>
    {
        private readonly IGroupRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteGroupCommandHandler(IGroupRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            clsGroup entity = await _repo.GetEntityBy(request.GroupID, enGroupType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.GroupNotFound);

            if (await _repo.HasActiveRelationships(entity.Id))
                return TResult<string>.Fail(ApplicationExMsg.GroupHasActiveRelationships);

            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");

            entity.SetDeleted(userID.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
