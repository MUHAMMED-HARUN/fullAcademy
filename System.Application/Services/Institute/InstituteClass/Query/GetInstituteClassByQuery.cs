using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.Program.DTO;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    public class GetInstituteClassByQuery : IRequest<TResult<InstituteClassInfo>>, IQuery
    {
        public string Param { get; set; }
        public enInstituteClassType ParamType { get; set; }
    }
}
