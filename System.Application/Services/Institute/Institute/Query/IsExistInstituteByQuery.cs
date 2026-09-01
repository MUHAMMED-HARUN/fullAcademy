

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Institute;
using System.Domain.Enums.Person;

namespace System.Application.Services.Institute.Institute.Query
{
    public class IsExistInstituteByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enInstituteType ParamType { get; set; }
    }
}
