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
    public class UpdateStudentStageCommandHandler : IRequestHandler<UpdateStudentStageCommand, TResult<string>>
    {
        private readonly IStudentStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateStudentStageCommandHandler(IStudentStageRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateStudentStageCommand request, CancellationToken cancellationToken)
        {
          clsStudentStage entity = await  _repo.GetEntityBy(request.StudentStageID, enStudentStageParamType.ID);
            if (entity == null)
                return TResult<string>.Fail( ApplicationExMsg.StudentStageNotFound);

           _mapper.Map<UpdateStudentStageCommand, clsStudentStage>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
