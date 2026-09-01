using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;

using System.Domain.Entities;

using System.Domain.Enums.Institute;


namespace System.Application.Services.Institute.InstituteClass.Command
{
    public class NewInstituteClassCommandHandler : IRequestHandler<NewInstituteClassCommand, TResult<string>>
    {
        private readonly IInstituteClassRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewInstituteClassCommandHandler(IInstituteClassRepository repo, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewInstituteClassCommand request, CancellationToken cancellationToken)
        {
            if ((await _repo.IsClassInInstituteBy(request.ClassID, request.InstituteID)) )
                return TResult<string>.Fail(ApplicationExMsg.failToFindProgram);

   

            clsInstituteClass entity = _mapper.Map<NewInstituteClassCommand, clsInstituteClass>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());


            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
