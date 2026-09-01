using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.StudentStage.DTO;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.StudentStage.Query
{
    public class GetStudentStageByQuery : IRequest<TResult<StudentStageInfo>>, IQuery
    {
        public string Param { get; set; }
        public enStudentStageParamType ParamType { get; set; }
    }
}
