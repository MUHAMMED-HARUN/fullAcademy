using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.DTO;

namespace System.Application.Services.Finance.FeeStructure.Query
{
    public class GetAllFeeStructureQuery : IRequest<TResult<List<FeeStructureInfo>>>, IQuery
    {
    }
}
