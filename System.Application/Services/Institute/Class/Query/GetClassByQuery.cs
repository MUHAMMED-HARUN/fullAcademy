using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.Class.DTO;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Class.Query
{
    public class GetClassByQuery : IRequest<TResult<ClassInfo>>, IQuery
    {
        public string Param { get; set; }
        public int ParamType { get; set; }
    }
}
