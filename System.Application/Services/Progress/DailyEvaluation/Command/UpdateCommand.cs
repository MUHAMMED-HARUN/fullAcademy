using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;
namespace System.Application.Services.Progress.DailyEvaluation.Command
{
    public class UpdateDailyEvaluationCommand : ICommand, IRequest<TResult<string>>
    {
        public string DailyEvaluationID { get; set; }
        public double To { get; set; }
        public enLevelType LevelType { get; set; }
    }
}
