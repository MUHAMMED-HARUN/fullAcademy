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
    public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, TResult<string>>
    {
        private readonly ISessionRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public DeleteSessionCommandHandler(ISessionRepository repo ,ICurrentUserService currentUser,IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
        {
            clsSession entity = await _repo.GetEntityBy(request.SessionID, enSessionParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindSession);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
