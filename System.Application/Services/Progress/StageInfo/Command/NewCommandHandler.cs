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
    public class NewStageInfoCommandHandler : IRequestHandler<NewStageInfoCommand, TResult<string>>
    {
        private readonly IStageInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewStageInfoCommandHandler(IStageInfoRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewStageInfoCommand request, CancellationToken cancellationToken)
        {
            clsStageInfo entity = _mapper.Map<NewStageInfoCommand, clsStageInfo>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());


            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
