using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Domain.Entities;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.StageInfo.Command
{
    public class DeleteStageInfoCommandHandler : IRequestHandler<DeleteStageInfoCommand, TResult<string>>
    {
        private readonly IStageInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        public DeleteStageInfoCommandHandler(IStageInfoRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(DeleteStageInfoCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            clsStageInfo? entity = await _repo.GetEntityBy(request.StageInfoID, enStageInfoParamType.ID);
            if (entity is null)
                return TResult<string>.Fail("لم يتم العثور على تفاصيل المرحلة");

            entity.SetDeleted(userID.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
