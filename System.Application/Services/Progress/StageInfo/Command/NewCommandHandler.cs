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
    public class NewStageInfoCommandHandler : IRequestHandler<NewStageInfoCommand, TResult<string>>
    {
        private readonly IStageInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStageRepository _stageRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        public NewStageInfoCommandHandler(
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
        public async Task<TResult<string>> Handle(NewStageInfoCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            if (await _stageRepository.GetEntityBy(request.StageID, enStageParamType.ID) is null)
                return TResult<string>.Fail("لم يتم العثور على المرحلة");

            var matter = await _sessionRepository.GetEntityBy(request.MatterID, enSessionParamType.ID);
            if (matter is null || matter.Type != enSessionType.Matter)
                return TResult<string>.Fail("اختر سجلاً صالحاً من نوع مادة");

            clsStageInfo entity = _mapper.Map<NewStageInfoCommand, clsStageInfo>(request);
            entity.SetCreated(userID.ToString());
            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail("تعذر إضافة تفاصيل المرحلة");

            return TResult<string>.OK(entityID);
        }
    }
}
