using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;
namespace System.Application.Services.Progress.DailyEvaluation.Command
{
    public class NewDailyEvaluationCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public double To { get;  set; }
        public DateTime Date { get;  set; }
        public string StageInfoID { get;  set; } = string.Empty;
        public enLevelType LevelType { get;  set; }
    }
}
