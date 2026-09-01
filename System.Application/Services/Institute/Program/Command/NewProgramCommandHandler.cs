using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Institute.Program.Command;
using System.Domain.Entities;
using System.Domain.Entities.Address;
using System.Domain.Entities.Institue;
using System.Domain.Enums.Institute;


namespace System.Application.Services.Institute.Program.Command
{
    public class NewProgramCommandHandler : IRequestHandler<NewProgramCommand, TResult<string>>
    {
        private readonly IProgramRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewProgramCommandHandler(IProgramRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewProgramCommand request, CancellationToken cancellationToken)
        {
            if ((await _repo.IsExistBy(request.ProgramName, enProgramType.Name)) == true)
                return TResult<string>.Fail(ApplicationExMsg.failToFindProgram);

   

            clsProgram entity = _mapper.Map<NewProgramCommand, clsProgram>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());


            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
