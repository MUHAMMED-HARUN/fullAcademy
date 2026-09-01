using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.District.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.District.Command
{
    public class UpdateDistrictCommandHandler : IRequestHandler<UpdateDistrictCommand, TResult<string>>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateDistrictCommandHandler(IDistrictRepository districtRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
        {
            DistrictInfo data  = await _districtRepository.GetById(request.DistrictID,enDistrictParamType.ID);
            if (data==null)
                return TResult<string>.Fail(ApplicationExMsg.DistrictNotFound);

            clsDistrict District = _mapper.Map<UpdateDistrictCommand, clsDistrict>(request);
            District.SetID(clsUtil.ToGuid(data.ID));
            District.SetModified( _currentUserService.UserId.ToString());

            await _districtRepository.Update(District);


            return TResult<string>.OK(string.Empty);

            
        }

   
    }
}
