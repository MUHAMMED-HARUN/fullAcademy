

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Progress.StageInfo.Command
{
    public class DeleteStageInfoCommand : ICommand, IRequest<TResult<string>>
    {
        public string StageInfoID { get; set; }
    }
}
