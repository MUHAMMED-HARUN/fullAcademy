using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Entities.Permissions;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.District.Command
{
    public class NewDistrictCommandHandler : IRequestHandler<NewDistrictCommand, TResult<string>>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewDistrictCommandHandler(
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
        public async Task<TResult<string>> Handle(NewDistrictCommand request, CancellationToken cancellationToken)
        {
            var city = await _cityRepository.GetById(request.CityID, enCityParamType.ID);
            if (city is null)
                return TResult<string>.Fail(ApplicationExMsg.CityNotFound);

            bool IsExist = await _districtRepository.IsNameExist(request.DistrictName);
            if (IsExist)
                return TResult<string>.Fail(ApplicationExMsg.DistrictIsExist);
            clsDistrict District = _mapper.Map<NewDistrictCommand,clsDistrict>(request);

            District.SetCreated(_currentUserService.UserId.ToString());
            string DistrictID = await _districtRepository.Add(District);

            if (string.IsNullOrEmpty(DistrictID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(DistrictID);
        }
    }
}
