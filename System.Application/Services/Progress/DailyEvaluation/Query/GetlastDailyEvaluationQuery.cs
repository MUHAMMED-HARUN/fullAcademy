using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.DailyEvaluation.DTO;
namespace System.Application.Services.Progress.DailyEvaluation.Query{
    public class GetlastDailyEvaluationQuery : IRequest<TResult<DailyEvaluationInfo?>>, IQuery
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StageInfoID { get; set; } = string.Empty;
    }
}
