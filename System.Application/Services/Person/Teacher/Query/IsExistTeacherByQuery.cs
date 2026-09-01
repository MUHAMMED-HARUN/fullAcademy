

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Teacher.Query
{
    public class IsExistTeacherByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enTeacherParamType ParamType { get; set; }
    }
}
