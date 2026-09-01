using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Institute;


namespace System.Application.Services.Institute.Program.Command
{
    public class UpdateProgramCommandHandler : IRequestHandler<UpdateProgramCommand, TResult<string>>
    {
        private readonly IProgramRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateProgramCommandHandler(IProgramRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateProgramCommand request, CancellationToken cancellationToken)
        {
          clsProgram entity = await  _repo.GetEntityBy(request.ProgramID, enProgramType.ID);
            if(entity == null )
                return TResult<string>.Fail(ApplicationExMsg.failToFindProgram);
            if ((await _repo.IsExistBy(request.ProgramName, enProgramType.Name)) 
                && entity.Name!=request.ProgramName)
                return TResult<string>.Fail(ApplicationExMsg.ProgramExistByName);

           _mapper.Map<UpdateProgramCommand, clsProgram>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
