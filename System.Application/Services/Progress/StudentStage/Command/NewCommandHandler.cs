using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;



namespace System.Application.Services.Progress.StudentStage.Command
{
    public class NewStudentStageCommandHandler : IRequestHandler<NewStudentStageCommand, TResult<string>>
    {
        private readonly IStudentStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewStudentStageCommandHandler(IStudentStageRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewStudentStageCommand request, CancellationToken cancellationToken)
        {
            clsStudentStage studentStage = await _repo.GetEntityBy(request.EnrollmentStudentID, request.StageID, enStageStateType.procecing);
            if (studentStage != null)
            {
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);
            }

   

            clsStudentStage entity = _mapper.Map<NewStudentStageCommand, clsStudentStage>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());


            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
