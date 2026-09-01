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
    
    public class NewDailyEvaluationCommandHandler : IRequestHandler<NewDailyEvaluationCommand, TResult<string>>
    {
        private readonly IDailyEvaluationRepository _repo;
        private readonly IStageInfoRepository _StageInforepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewDailyEvaluationCommandHandler(IDailyEvaluationRepository repo, ICurrentUserService currentUser,
            IMapper mapper,IStageInfoRepository infoRepository)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
            _StageInforepo = infoRepository;
        }
        public async Task<TResult<string>> Handle(NewDailyEvaluationCommand request, CancellationToken cancellationToken)
        {
            var LastdailyEvaluation = await _repo.GetLastDailyEvaluationInfo(request.StudentEnrollmentID,request.StageInfoID);
            clsStageInfo stageInfo = await _StageInforepo.GetEntityBy(request.StageInfoID, enStageInfoParamType.ID);
            if (stageInfo == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindStageInfo);
            double from = stageInfo.From;
            if (LastdailyEvaluation != null)
            {
                from = LastdailyEvaluation.To+1;
                if(LastdailyEvaluation.To>=request.To)
                    return TResult<string>.Fail(ApplicationExMsg.DailyEvaluationRangeIsOutOFRange);

                if (!(request.To > LastdailyEvaluation.To &&
                   stageInfo.To >= request.To &&
                   stageInfo.From <= request.To))
                return TResult<string>.Fail(ApplicationExMsg.DailyEvaluationRangeIsOutOFRange);
            }
        

            clsDailyEvaluation entity = _mapper.Map<NewDailyEvaluationCommand, clsDailyEvaluation>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());
            entity.SetFrom(from);

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
