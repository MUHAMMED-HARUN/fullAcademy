

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Person.Student.DTO;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Student.Query
{
    public class GetStudentByQuery : IRequest<TResult<StudentInfo>>, IQuery
    {
        public string Param { get; set; }
        public enStudentParamType ParamType { get; set; }
    }
}
