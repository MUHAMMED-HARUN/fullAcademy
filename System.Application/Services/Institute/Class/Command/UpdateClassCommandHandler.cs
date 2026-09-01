using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Institute;


namespace System.Application.Services.Institute.Class.Command
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, TResult<string>>
    {
        private readonly IClassRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateClassCommandHandler(IClassRepository Repository, ICurrentUserService currentUser,
            IMapper mapper, IAddressRepository addressRepository)
        {
            _repo = Repository;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
          clsClass entity= await  _repo.GetEntityBy(request.ClassID, enClassType.ID);
            if(entity == null)
                return TResult<string>.Fail(ApplicationExMsg.failUpdateClass);
            if ((await _repo.IsAvalibleName(request.ClassName,request.ClassLevel ,request.ProgramID)) 
                &&entity.Name==request.ClassName&& request.ProgramID == entity.ProgramID.ToString())
                return TResult<string>.Fail(ApplicationExMsg.ClassExistByName);

            bool AvilableLevel = await _repo.IsAvalibleLevel(request.ClassLevel, request.ProgramID);
            if (AvilableLevel && (entity.Level != request.ClassLevel && entity.ProgramID ==clsUtil.ToGuid( request.ProgramID)))
                return TResult<string>.Fail(ApplicationExMsg.failToFindClass);

            _mapper.Map<UpdateClassCommand, clsClass>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());


            await _repo.Update(entity);

           ;

            return TResult<string>.OK(string.Empty);
        }
    }
}
