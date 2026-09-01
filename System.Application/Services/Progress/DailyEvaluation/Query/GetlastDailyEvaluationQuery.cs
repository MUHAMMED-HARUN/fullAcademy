using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.DailyEvaluation.DTO;
namespace System.Application.Services.Progress.DailyEvaluation.Query{
    public class GetlastDailyEvaluationQuery : IRequest<TResult<DTO.DailyEvaluationInfo>>, IQuery
    {
        public string StudentEnrollmentID { get; set; }
        public string StageInfoID { get; set; }
    }
}
