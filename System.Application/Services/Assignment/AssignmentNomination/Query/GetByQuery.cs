using MediatR;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScope.DTO;

namespace System.Application.Services.Assignment.AssignmentNomination.Query
{
    public class GetAssignmentNominationByQuery : IRequest<TResult<DTO.AssignmentNominationInfo>>
    {
        public string AssignmentNominationID { get; set; }
    }
}
