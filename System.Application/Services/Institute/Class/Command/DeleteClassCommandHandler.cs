using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Class.Command;
using System.Application.Services.Institute.Institute.Command;
using System.Domain.Entities;
using System.Domain.Entities.Institue;
using System.Domain.Enums.Institute;

namespace System.Application.Institute.Class.Command
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, TResult<string>>
    {
        private readonly IClassRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public DeleteClassCommandHandler(IClassRepository repo ,ICurrentUserService currentUser,IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            clsClass entity = await _repo.GetEntityBy(request.ClassID, enClassType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindClass);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
