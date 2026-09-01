using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;


namespace System.Application.Services.Institute.Class.Command
{
    public class NewClassCommandHandler : IRequestHandler<NewClassCommand, TResult<string>>
    {
        private readonly IClassRepository _Repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewClassCommandHandler(IClassRepository Repository, ICurrentUserService currentUser,
            IMapper mapper,IAddressRepository addressRepository)
        {
            _Repo= Repository;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewClassCommand request, CancellationToken cancellationToken)
        {
            bool AvilabelName = await _Repo.IsAvalibleName(request.ClassName, request.ClassLevel, request.ProgramID);
            bool AvilableLevel = await _Repo.IsAvalibleLevel(request.ClassLevel, request.ProgramID);
            if (AvilableLevel || AvilabelName) 
                return TResult<string>.Fail(ApplicationExMsg.failToFindClass);

            clsClass entity = _mapper.Map<NewClassCommand, clsClass>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());


            string entityID = await _Repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
