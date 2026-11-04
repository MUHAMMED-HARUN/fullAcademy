using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.Session.Command
{
    public class UpdateSessionCommandHandler : IRequestHandler<UpdateSessionCommand, TResult<string>>
    {
        private readonly ISessionRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateSessionCommandHandler(ISessionRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userId)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            clsSession? entity = await _repo.GetEntityBy(request.SessionID, enSessionParamType.ID);
            if (entity is null)
                return TResult<string>.Fail("لم يتم العثور على السجل");

            if (await _repo.IsExistNameByType(request.SessionName, request.SessionType, entity.Id))
                return TResult<string>.Fail("يوجد سجل بالاسم والنوع نفسيهما");

            if (entity.Type != request.SessionType && await _repo.HasChildren(entity.Id))
                return TResult<string>.Fail("لا يمكن تغيير نوع السجل لوجود سجلات أبناء مرتبطة به");

            if (!string.IsNullOrWhiteSpace(request.ParentSessionID))
            {
                var parent = await _repo.GetEntityBy(request.ParentSessionID, enSessionParamType.ID);
                if (parent is null)
                    return TResult<string>.Fail("لم يتم العثور على السجل الأب");

                if (parent.Id == entity.Id || parent.Type != enSessionType.Subject ||
                    request.SessionType != enSessionType.Matter)
                    return TResult<string>.Fail("يجب أن يكون الأب موضوعاً وأن يكون الابن مادة");
            }

            _mapper.Map<UpdateSessionCommand, clsSession>(request, entity);
            entity.SetModified(userId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
