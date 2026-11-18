using MediatR;
using System.Application.Common;
using System.Application.Common.Models;
using System.Application.Services.Assignment.AssignmentNomination.DTO;

namespace System.Application.Services.Assignment.AssignmentNomination.Query
{
    public sealed class GetAllAssignmentNominationQuery : BaseFilterQuery<AssignmentNominationInfo>
    {
    }
}
