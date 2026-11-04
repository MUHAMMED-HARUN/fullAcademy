using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.Session.Command
{
    public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, TResult<string>>
    {
        private readonly ISessionRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        public DeleteSessionCommandHandler(ISessionRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userId)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            clsSession? entity = await _repo.GetEntityBy(request.SessionID, enSessionParamType.ID);
            if (entity is null)
                return TResult<string>.Fail("لم يتم العثور على السجل");

            if (await _repo.HasChildren(entity.Id))
                return TResult<string>.Fail("لا يمكن حذف سجل له سجلات أبناء؛ احذف الأبناء أولاً");

            entity.SetDeleted(userId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
