using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.DTO;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Institute.Query
{
    public class GetInstituteByQuery : IRequest<TResult<InstituteInfo>>, IQuery
    {
        public string Param { get; set; }
        public enInstituteType ParamType { get; set; }
    }
}
