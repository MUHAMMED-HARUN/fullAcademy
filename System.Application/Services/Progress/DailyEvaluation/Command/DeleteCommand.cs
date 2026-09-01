

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Progress.DailyEvaluation.Command
{
    public class DeleteDailyEvaluationCommand : ICommand, IRequest<TResult<string>>
    {
        public string DailyEvaluationID { get; set; }
    }
}
