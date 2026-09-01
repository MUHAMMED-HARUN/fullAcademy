using MediatR;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.DTO;

namespace System.Application.Services.Assignment.Assignment.Query
{
    public class GetAssignmentByQuery : IRequest<TResult<AssignmentInfo>>
    {
        public string ID { get; set; }
    }

    public class GetAllAssignmentQuery : IRequest<TResult<List<AssignmentInfo>>>
    {
    }
}
