using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Institute;
using System.Domain.Enums.Person;



namespace System.Application.Services.Progress.Stage.Command
{
    public class NewStageCommandHandler : IRequestHandler<NewStageCommand, TResult<string>>
    {
        private readonly IStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;
        public NewStageCommandHandler(
            IStageRepository repo,
            IClassRepository classRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _classRepository = classRepository;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewStageCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userId)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            Guid classID = clsUtil.ToGuid(request.ClassID);
            Guid? previousStageID = string.IsNullOrWhiteSpace(request.PrevStageID)
                ? null
                : clsUtil.ToGuid(request.PrevStageID);

            if (!await _classRepository.IsExistBy(request.ClassID, enClassType.ID))
                return TResult<string>.Fail("الصف المحدد غير موجود");

            if (!await _repo.IsValidPreviousStage(classID, previousStageID))
                return TResult<string>.Fail("المرحلة السابقة غير صالحة لهذا الصف");

            if (await _repo.IsNameExistInClass(request.StageName, classID))
                return TResult<string>.Fail("توجد مرحلة بهذا الاسم في الصف المحدد");

            clsStage entity = _mapper.Map<NewStageCommand, clsStage>(request);
            entity.SetCreated(userId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail("تعذر إضافة المرحلة");


            return TResult<string>.OK(entityID);
        }
    }
}
