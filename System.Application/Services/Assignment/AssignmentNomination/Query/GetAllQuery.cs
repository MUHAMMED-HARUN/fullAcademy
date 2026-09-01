using MediatR;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentNomination.Query
{
    public class GetAllAssignmentNominationQuery : IRequest<TResult<List<DTO.AssignmentNominationInfo>>>
    {
    }
}
