using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.Stage.Command
{
    public class DeleteStageCommandHandler : IRequestHandler<DeleteStageCommand, TResult<string>>
    {
        private readonly IStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        public DeleteStageCommandHandler(IStageRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(DeleteStageCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userId)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            clsStage? entity = await _repo.GetEntityBy(request.StageID, enStageParamType.ID);
            if (entity is null)
                return TResult<string>.Fail("لم يتم العثور على المرحلة");

            entity.SetDeleted(userId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
