using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Institute.Command;
using System.Domain.Entities;
using System.Domain.Enums.Institute;

namespace System.Application.Institute.Program.Command
{
    public class DeleteProgramCommandHandler : IRequestHandler<DeleteProgramCommand, TResult<string>>
    {
        private readonly IProgramRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public DeleteProgramCommandHandler(IProgramRepository repo ,ICurrentUserService currentUser,IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(DeleteProgramCommand request, CancellationToken cancellationToken)
        {
            clsProgram entity = await _repo.GetEntityBy(request.ProgramID, enProgramType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindProgram);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
