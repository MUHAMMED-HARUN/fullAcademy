

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Progress.Stage.Command
{
    public class DeleteStageCommand : ICommand, IRequest<TResult<string>>
    {
        public string StageID { get; set; }
    }
}
