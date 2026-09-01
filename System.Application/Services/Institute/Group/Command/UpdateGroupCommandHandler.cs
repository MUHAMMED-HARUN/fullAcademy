using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Group.Command
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, TResult<string>>
    {
        private readonly IGroupRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateGroupCommandHandler(IGroupRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            clsGroup entity = await _repo.GetEntityBy(request.GroupID, enGroupType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.GroupNotFound);

            if ((await _repo.IsAvalibleName(request.GroupName, request.InstituteClassID)) && entity.GroupName != request.GroupName)
                return TResult<string>.Fail(ApplicationExMsg.GroupExistByName);

            if ((await _repo.IsAvalibleCode(request.Code, request.InstituteClassID)) && entity.Code != request.Code)
                return TResult<string>.Fail(ApplicationExMsg.GroupExistByCode);

            _mapper.Map<UpdateGroupCommand, clsGroup>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
