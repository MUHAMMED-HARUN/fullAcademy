using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.DailyEvaluation.Command
{
    public class UpdateDailyEvaluationCommandHandler : IRequestHandler<UpdateDailyEvaluationCommand, TResult<string>>
    {
        private readonly IDailyEvaluationRepository _repo;
        private readonly IStageInfoRepository _StageInforepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateDailyEvaluationCommandHandler(IDailyEvaluationRepository repo, ICurrentUserService currentUser,
            IMapper mapper, IStageInfoRepository infoRepository)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
            _StageInforepo = infoRepository;
        }
        public async Task<TResult<string>> Handle(UpdateDailyEvaluationCommand request, CancellationToken cancellationToken)
        {

            clsDailyEvaluation entity = await _repo.GetEntityBy( request.DailyEvaluationID,enDailyEvaluationParamType.ID);
            clsDailyEvaluation lastDailyEvaluation = await _repo.GetLastDailyEvaluation(entity.StudentEnrollmentID.ToString(), request.DailyEvaluationID);
            clsStageInfo stageInfo = await _StageInforepo.GetEntityBy(entity.StageInfoID.ToString(), enStageInfoParamType.ID);

            if (stageInfo == null || entity == null || lastDailyEvaluation == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindStageInfo);


            if (!(request.To > lastDailyEvaluation.To &&
                   request.To <stageInfo.To&&
                   stageInfo.From < request.To))
                return TResult<string>.Fail(ApplicationExMsg.DailyEvaluationRangeIsOutOFRange);


            _mapper.Map<UpdateDailyEvaluationCommand, clsDailyEvaluation>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
