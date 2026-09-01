

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Person.Teacher.DTO;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Teacher.Query
{
    public class GetTeacherByQuery : IRequest<TResult<TeacherInfo>>, IQuery
    {
        public string Param { get; set; }
        public enTeacherParamType ParamType { get; set; }
    }
}
