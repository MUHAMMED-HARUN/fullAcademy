using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Group.Query
{
    public class IsExistGroupByQuery : IRequest<TResult<bool>>, IQuery
    {
        public string Param { get; set; }
        public enGroupType ParamType { get; set; }
    }
}
