using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
namespace System.Application.Services.Progress.Stage.Command
{
    public class NewStageCommand : ICommand, IRequest<TResult<string>>
    {
        public string StageName { get; set; } = string.Empty;
        public string? StageNote { get; set; }
        public string ClassID { get; set; } = string.Empty;
        public string? PrevStageID { get; set; }
    }
}
