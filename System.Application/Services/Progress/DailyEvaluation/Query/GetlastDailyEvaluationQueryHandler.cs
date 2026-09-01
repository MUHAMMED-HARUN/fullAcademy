using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;


namespace System.Application.Services.Progress.DailyEvaluation.Query
{
    class GetlastDailyEvaluationQueryHandler : IRequestHandler<GetlastDailyEvaluationQuery, TResult<DTO.DailyEvaluationInfo>>
    {
        IDailyEvaluationRepository _repo;
        public GetlastDailyEvaluationQueryHandler( IDailyEvaluationRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<DTO.DailyEvaluationInfo>> Handle(GetlastDailyEvaluationQuery request, CancellationToken cancellationToken)
        {
             return TResult<DTO.DailyEvaluationInfo>.OK(await _repo.GetLastDailyEvaluationInfo(request.StudentEnrollmentID,request.StageInfoID));
        }
    }
}
