using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.AssignmentNomination.DTO;

namespace System.Application.Services.Assignment.AssignmentNomination.Query
{
    public class GetAssignmentNominationByQueryHandler : IRequestHandler<GetAssignmentNominationByQuery, TResult<DTO.AssignmentNominationInfo>>
    {
        private readonly IAssignmentNominationRepository _repo;

        public GetAssignmentNominationByQueryHandler(IAssignmentNominationRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<DTO.AssignmentNominationInfo>> Handle(GetAssignmentNominationByQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetBy(clsUtil.ToGuid(request.AssignmentNominationID));
            if (result == null)
                return TResult<DTO.AssignmentNominationInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<DTO.AssignmentNominationInfo>.OK(result);
        }
    }
}
