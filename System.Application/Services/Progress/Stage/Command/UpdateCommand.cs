using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
namespace System.Application.Services.Progress.Stage.Command
{
    public class UpdateStageCommand : ICommand, IRequest<TResult<string>>
    {
        public string StageID {get; set;}
        public string StageName { get; set; }
        public string StageNote { get; set; }
        public string ProgramID { get; set; }
        public string PrevStageID { get; set; }
    }
}
