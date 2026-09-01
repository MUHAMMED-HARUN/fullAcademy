using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.StudentStage.Query
{
    public class IsExistStudentStageByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enStudentStageParamType ParamType { get; set; }
    }
}
