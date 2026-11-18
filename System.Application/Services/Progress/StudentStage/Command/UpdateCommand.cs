using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;
namespace System.Application.Services.Progress.StudentStage.Command
{
    public class UpdateStudentStageCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentStageID { get; set; } = string.Empty;
        public enStageStateType StudentStageState { get; set; }
        public DateOnly StudentStageDate { get; set; }
    }
}
