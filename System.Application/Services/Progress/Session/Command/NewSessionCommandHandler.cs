using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;



namespace System.Application.Services.Progress.Session.Command
{
    public class NewSessionCommandHandler : IRequestHandler<NewSessionCommand, TResult<string>>
    {
        private readonly ISessionRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewSessionCommandHandler(ISessionRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewSessionCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is not Guid userId)
                return TResult<string>.Fail("غير مصرح بهذا الإجراء");

            if (await _repo.IsExistNameByType(request.SessionName, request.SessionType))
            {
                return TResult<string>.Fail("يوجد سجل بالاسم والنوع نفسيهما");
            }

            if (!string.IsNullOrWhiteSpace(request.ParentSessionID))
            {
                var parent = await _repo.GetEntityBy(request.ParentSessionID, enSessionParamType.ID);
                if (parent is null)
                    return TResult<string>.Fail("لم يتم العثور على السجل الأب");

                if (parent.Type != enSessionType.Subject || request.SessionType != enSessionType.Matter)
                    return TResult<string>.Fail("يجب أن يكون الأب موضوعاً وأن يكون الابن مادة");
            }

            clsSession entity = _mapper.Map<NewSessionCommand, clsSession>(request);
            entity.SetCreated(userId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
