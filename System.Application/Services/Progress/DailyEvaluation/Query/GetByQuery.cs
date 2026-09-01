using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.DailyEvaluation.Query
{
    public class GetDailyEvaluationByQuery : IRequest<TResult<DTO.DailyEvaluationInfo>>, IQuery
    {
        public string Param { get; set; }
        public enDailyEvaluationParamType ParamType { get; set; }
    }
}
