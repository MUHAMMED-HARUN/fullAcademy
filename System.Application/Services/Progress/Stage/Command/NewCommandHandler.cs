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
    public class NewStageCommandHandler : IRequestHandler<NewStageCommand, TResult<string>>
    {
        private readonly IStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewStageCommandHandler(IStageRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewStageCommand request, CancellationToken cancellationToken)
        {

            if ((await _repo.IsExistBy(request.StageName, enStageParamType.Name)) == true)
            {
                clsStage Stage = await _repo.GetEntityBy(request.StageName, enStageParamType.Name);
                if (Stage != null && Stage.Name == request.StageName && Stage.ProgramID == clsUtil.ToGuid(request.ProgramID))
                    return TResult<string>.Fail(ApplicationExMsg.StageNameExistInProgram);
            }




            clsStage entity = _mapper.Map<NewStageCommand, clsStage>(request);

            entity.SetCreated(_currentUserService.UserId.ToString());


            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);


            return TResult<string>.OK(entityID);
        }

    } }
   