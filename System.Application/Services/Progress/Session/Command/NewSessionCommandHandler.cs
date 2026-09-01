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
            if ((await _repo.IsExistBy(request.SessionName, enSessionParamType.Name)) == true)
            {
                clsSession session = await _repo.GetEntityBy(request.SessionName, enSessionParamType.Name);
                if (session != null && session.Type == session.Type)
                    return TResult<string>.Fail(ApplicationExMsg.SessionNameExistInType);
            }
            if (!string.IsNullOrEmpty(request.ParentSessionID))
            {
                clsSession session1 = await _repo.GetEntityBy(request.ParentSessionID, enSessionParamType.ID);
                if (session1 != null)
                    if (session1.Type == enSessionType.Matter||request.SessionType==enSessionType.Subject)
                        return TResult<string>.Fail(ApplicationExMsg.SessionParentIsMatter);
            }

   

            clsSession entity = _mapper.Map<NewSessionCommand, clsSession>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());


            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
