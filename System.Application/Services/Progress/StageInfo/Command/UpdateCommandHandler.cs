using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Domain.Entities;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.StageInfo.Command
{
    public class UpdateStageInfoCommandHandler : IRequestHandler<UpdateStageInfoCommand, TResult<string>>
    {
        private readonly IStageInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStageRepository _stageRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        public UpdateStageInfoCommandHandler(
            IStageInfoRepository repo,
            IStageRepository stageRepository,
            ISessionRepository sessionRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _stageRepository = stageRepository;
            _sessionRepository = sessionRepository;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateStageInfoCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            clsStageInfo? entity = await _repo.GetEntityBy(request.StageInfoID, enStageInfoParamType.ID);
            if (entity is null)
                return TResult<string>.Fail("لم يتم العثور على تفاصيل المرحلة");

            if (await _stageRepository.GetEntityBy(request.StageID, enStageParamType.ID) is null)
                return TResult<string>.Fail("لم يتم العثور على المرحلة");

            var matter = await _sessionRepository.GetEntityBy(request.MatterID, enSessionParamType.ID);
            if (matter is null || matter.Type != enSessionType.Matter)
                return TResult<string>.Fail("اختر سجلاً صالحاً من نوع مادة");

            _mapper.Map<UpdateStageInfoCommand, clsStageInfo>(request, entity);
            entity.SetModified(userID.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
