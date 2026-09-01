using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
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
          clsSession entity = await  _repo.GetEntityBy(request.SessionID, enSessionParamType.ID);
            if(entity == null )
                return TResult<string>.Fail(ApplicationExMsg.failToFindSession);
            if ((await _repo.IsExistBy(request.SessionName, enSessionParamType.Name)) 
                && entity.Name!=request.SessionName)
                return TResult<string>.Fail(ApplicationExMsg.SessionExistByName);

            if (!string.IsNullOrEmpty(request.ParentSessionID))
            {
                clsSession session1 = await _repo.GetEntityBy(request.ParentSessionID, enSessionParamType.ID);
                if (session1 != null)
                    if (session1.Type == enSessionType.Matter || request.SessionType == enSessionType.Subject)
                        return TResult<string>.Fail(ApplicationExMsg.SessionParentIsMatter);
            }
            _mapper.Map<UpdateSessionCommand, clsSession>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
