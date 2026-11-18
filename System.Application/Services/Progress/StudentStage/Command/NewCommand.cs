using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;
namespace System.Application.Services.Progress.StudentStage.Command
{
    public class NewStudentStageCommand : ICommand, IRequest<TResult<string>>
    {
        public string StageID { get; set; } = string.Empty;
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public enStageStateType StudentStageState { get; set; }
        public DateOnly StudentStageDate { get; set; }
    }
}
