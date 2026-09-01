using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Progress.DailyEvaluation.Query
{
    class IsExistDailyEvaluationByQueryHandler : IRequestHandler<IsExistDailyEvaluationByQuery, TResult<bool>>
    {
        IDailyEvaluationRepository _repo;
        public IsExistDailyEvaluationByQueryHandler(   IDailyEvaluationRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistDailyEvaluationByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindDailyEvaluation);

            return TResult<bool>.OK(isExist);

        }
    }
}
