

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Progress.StudentStage.Command
{
    public class DeleteStudentStageCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentStageID { get; set; }
    }
}
