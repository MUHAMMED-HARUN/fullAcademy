using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Group.Command;
using System.Domain.Entities;

namespace System.Application.Services.Institute.Group.Command
{
    public class NewGroupCommandHandler : IRequestHandler<NewGroupCommand, TResult<string>>
    {
        private readonly IGroupRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewGroupCommandHandler(IGroupRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewGroupCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.IsAvalibleName(request.GroupName, request.InstituteClassID))
                return TResult<string>.Fail(ApplicationExMsg.GroupExistByName);

            if (await _repo.IsAvalibleCode(request.Code, request.InstituteClassID))
                return TResult<string>.Fail(ApplicationExMsg.GroupExistByCode);

            clsGroup entity = _mapper.Map<NewGroupCommand, clsGroup>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
