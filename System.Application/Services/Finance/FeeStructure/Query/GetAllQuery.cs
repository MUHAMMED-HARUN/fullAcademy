using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.DTO;
using System.Application.Common.Models;

namespace System.Application.Services.Finance.FeeStructure.Query
{
    public class GetAllFeeStructureQuery : IRequest<TResult<List<FeeStructureInfo>>>, IQuery
    {
    }

    public sealed class GetAllFeeStructureFilterQuery : BaseFilterQuery<FeeStructureInfo>
    {
        public GetAllFeeStructureFilterQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
