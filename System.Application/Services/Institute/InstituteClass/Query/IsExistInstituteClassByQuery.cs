using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    public class IsExistInstituteClassByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enInstituteClassType ParamType { get; set; }
    }
}
