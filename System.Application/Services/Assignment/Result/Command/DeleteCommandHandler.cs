using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.Result.Command;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Result.Command
{
    public class DeleteResultCommandHandler : IRequestHandler<DeleteResultCommand, TResult<string>>
    {
        private readonly IResultRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteResultCommandHandler(IResultRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteResultCommand request, CancellationToken cancellationToken)
        {
            clsResult entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.ResultID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
