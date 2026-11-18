using MediatR;
using System.Application.Common;
using System.Application.Services.Assignment.Result.DTO;
using System.Application.Common.Models;

namespace System.Application.Services.Assignment.Result.Query
{
    public class GetResultByQuery : IRequest<TResult<ResultInfo>>
    {
        public string ID { get; set; } = string.Empty;
    }

    public sealed class GetAllResultQuery : BaseFilterQuery<ResultInfo>
    {
        public string? AssignmentID { get; set; }
    }

    public sealed class GetResultForUpsertQuery : IRequest<TResult<ResultInfo>>
    {
        public string ResultID { get; set; } = string.Empty;
    }
}
