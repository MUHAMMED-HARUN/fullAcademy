using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.InstituteClass.Command;
using System.Domain.Entities;
using System.Domain.Enums.Institute;

namespace System.Application.Institute.InstituteClass.Command
{
    public class DeleteInstituteClassCommandHandler : IRequestHandler<DeleteInstituteClassCommand, TResult<string>>
    {
        private readonly IInstituteClassRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public DeleteInstituteClassCommandHandler(IInstituteClassRepository repo ,ICurrentUserService currentUser,IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(DeleteInstituteClassCommand request, CancellationToken cancellationToken)
        {
            clsInstituteClass entity = await _repo.GetEntityBy(request.InstituteClassID, enInstituteClassType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindInstituteClass);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
