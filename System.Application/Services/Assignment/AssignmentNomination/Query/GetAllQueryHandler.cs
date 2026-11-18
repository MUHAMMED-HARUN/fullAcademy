using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentNomination.DTO;
using Gridify;

namespace System.Application.Services.Assignment.AssignmentNomination.Query
{
    public class GetAllAssignmentNominationQueryHandler : IRequestHandler<GetAllAssignmentNominationQuery, TResult<Paging<AssignmentNominationInfo>>>
    {
        private readonly IAssignmentNominationRepository _repo;

        public GetAllAssignmentNominationQueryHandler(IAssignmentNominationRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<AssignmentNominationInfo>>> Handle(GetAllAssignmentNominationQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<AssignmentNominationInfo>>.OK(await _repo.GetListFilter(request));
        }
    }
}
