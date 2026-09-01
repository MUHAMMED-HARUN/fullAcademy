using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.Stage.Command
{
    public class UpdateStageCommandHandler : IRequestHandler<UpdateStageCommand, TResult<string>>
    {
        private readonly IStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateStageCommandHandler(IStageRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateStageCommand request, CancellationToken cancellationToken)
        {
          clsStage entity = await  _repo.GetEntityBy(request.StageID, enStageParamType.ID);
            if(entity == null )
                return TResult<string>.Fail(ApplicationExMsg.failToFindStage);
            if ((await _repo.IsExistBy(request.StageName, enStageParamType.Name)) 
                && entity.Name!=request.StageName)
                return TResult<string>.Fail(ApplicationExMsg.StageExistByName);

           _mapper.Map<UpdateStageCommand, clsStage>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
