using MediatR;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentNomination.Query
{
    public class GetAssignmentNominationByQuery : IRequest<TResult<DTO.AssignmentNominationInfo>>
    {
        public string AssignmentNominationID { get; set; } = string.Empty;
    }
}
