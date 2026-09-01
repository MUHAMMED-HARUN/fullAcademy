using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Progress.DailyEvaluation.DTO;
using System.Application.Services.Progress.DailyEvaluation.Query;


namespace System.Application.Services.Institute.Institute.Query
{
    class GetDailyEvaluationByQueryHandler : IRequestHandler<GetDailyEvaluationByQuery, TResult<DailyEvaluationInfo>>
    {
        IDailyEvaluationRepository _repo;
        public GetDailyEvaluationByQueryHandler( IDailyEvaluationRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<DailyEvaluationInfo>> Handle(GetDailyEvaluationByQuery request, CancellationToken cancellationToken)
        {
            DailyEvaluationInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<DailyEvaluationInfo>.Fail(ApplicationExMsg.failToFindDailyEvaluation);

            return TResult<DailyEvaluationInfo>.OK(entityInfo);

        }
    }
}
