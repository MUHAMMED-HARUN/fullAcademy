

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Student.Query
{
    public class IsExistStudentByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enStudentParamType ParamType { get; set; }
    }
}
