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
        private readonly ICityRepository _cityRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateDistrictCommandHandler(
            IDistrictRepository districtRepository,
            ICityRepository cityRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _districtRepository = districtRepository;
            _cityRepository = cityRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
        {
            var data = await _districtRepository.GetById(request.DistrictID, enDistrictParamType.ID);
            if (data==null)
                return TResult<string>.Fail(ApplicationExMsg.DistrictNotFound);

            var districtID = clsUtil.ToGuid(request.DistrictID);
            var city = await _cityRepository.GetById(request.CityID, enCityParamType.ID);
            if (city is null)
                return TResult<string>.Fail(ApplicationExMsg.CityNotFound);

            if (await _districtRepository.IsNameExist(request.DistrictName, districtID))
                return TResult<string>.Fail(ApplicationExMsg.DistrictIsExist);

            clsDistrict District = _mapper.Map<UpdateDistrictCommand, clsDistrict>(request);
            District.SetID(districtID);
            District.SetModified( _currentUserService.UserId.ToString());

            await _districtRepository.Update(District);


            return TResult<string>.OK(string.Empty);

            
        }

   
    }
}
