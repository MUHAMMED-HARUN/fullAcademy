using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.StageInfo.Command
{
    public class DeleteStageInfoCommandHandler : IRequestHandler<DeleteStageInfoCommand, TResult<string>>
    {
        private readonly IStageInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public DeleteStageInfoCommandHandler(IStageInfoRepository repo ,ICurrentUserService currentUser,IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(DeleteStageInfoCommand request, CancellationToken cancellationToken)
        {
            clsStageInfo entity = await _repo.GetEntityBy(request.StageInfoID, enStageInfoParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindStageInfo);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
