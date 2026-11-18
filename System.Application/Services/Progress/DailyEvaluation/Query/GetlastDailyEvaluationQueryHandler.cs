using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.DailyEvaluation.DTO;


namespace System.Application.Services.Progress.DailyEvaluation.Query
{
    class GetlastDailyEvaluationQueryHandler : IRequestHandler<GetlastDailyEvaluationQuery, TResult<DailyEvaluationInfo?>>
    {
        IDailyEvaluationRepository _repo;
        public GetlastDailyEvaluationQueryHandler( IDailyEvaluationRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<DailyEvaluationInfo?>> Handle(GetlastDailyEvaluationQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetLastDailyEvaluationInfo(
                request.StudentAcademicEnrollmentID,
                request.StageInfoID);
            return result is null
                ? TResult<DailyEvaluationInfo?>.OK(null)
                : TResult<DailyEvaluationInfo?>.OK(result);
        }
    }
}
