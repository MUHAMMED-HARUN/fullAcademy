using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Group.Query
{
    public class GetGroupByQuery : IRequest<TResult<GroupInfo>>, IQuery
    {
        public string Param { get; set; }
        public enGroupType ParamType { get; set; }
    }
}
