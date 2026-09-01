using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Assignment.AssignmentNomination.Query
{
    public class GetAllAssignmentNominationQueryHandler : IRequestHandler<GetAllAssignmentNominationQuery, TResult<List<DTO.AssignmentNominationInfo>>>
    {
        private readonly IAssignmentNominationRepository _repo;

        public GetAllAssignmentNominationQueryHandler(IAssignmentNominationRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<DTO.AssignmentNominationInfo>>> Handle(GetAllAssignmentNominationQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetList();
            if (result == null || result.Count == 0)
                return TResult<List<DTO.AssignmentNominationInfo>>.Fail(ApplicationExMsg.NotFound);

            return TResult<List<DTO.AssignmentNominationInfo>>.OK(result);
        }
    }
}
