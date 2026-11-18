using Gridify;
using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.DailyEvaluation.DTO;


namespace System.Application.Services.Progress.DailyEvaluation.Query
{
    class GetAllDailyEvaluationQueryHandler : IRequestHandler<GetAllDailyEvaluationQuery, TResult<Paging<DailyEvaluationInfo>>>
    {
        IDailyEvaluationRepository _repo;

        public GetAllDailyEvaluationQueryHandler(IDailyEvaluationRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<Paging<DailyEvaluationInfo>>> Handle(GetAllDailyEvaluationQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<DailyEvaluationInfo>>.OK(await _repo.GetAllFilter(request));
        }
    }
}
