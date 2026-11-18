using MediatR;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.DTO;
using System.Application.Common.Models;

namespace System.Application.Services.Assignment.Assignment.Query
{
    public class GetAssignmentByQuery : IRequest<TResult<AssignmentInfo>>
    {
        public string ID { get; set; } = string.Empty;
    }

    public sealed class GetAllAssignmentQuery : BaseFilterQuery<AssignmentInfo>
    {
    }

    public sealed class GetAssignmentForUpsertQuery : IRequest<TResult<AssignmentInfo>>
    {
        public string AssignmentID { get; set; } = string.Empty;
    }
}
