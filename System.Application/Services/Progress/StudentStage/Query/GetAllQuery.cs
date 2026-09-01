using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.StudentStage.DTO;
namespace System.Application.Services.Progress.StudentStage.Query{
    public class GetAllStudentStageQuery : IRequest<TResult<List<StudentStageInfo>>>, IQuery
    {
    }
}
