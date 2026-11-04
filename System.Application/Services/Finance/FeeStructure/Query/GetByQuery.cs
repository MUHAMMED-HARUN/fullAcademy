using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.DTO;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.FeeStructure.Query
{
    public class GetFeeStructureByQuery : IRequest<TResult<FeeStructureInfo>>, IQuery
    {
        public string Param { get; set; }
        public enFeeStructureParamType ParamType { get; set; }
    }
}
