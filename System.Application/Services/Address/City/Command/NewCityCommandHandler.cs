using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Entities.Permissions;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.City.Command
{
    public class NewCityCommandHandler : IRequestHandler<NewCityCommand, TResult<string>>
    {
        private readonly ICityRepository _CityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewCityCommandHandler(
            ICityRepository cityRepository,
            ICountryRepository countryRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _CityRepository = cityRepository;
            _countryRepository = countryRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewCityCommand request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetById(request.CountryID, enCountryParamType.ID);
            if (country is null)
                return TResult<string>.Fail(ApplicationExMsg.CountryNotFound);

            bool IsExist = await _CityRepository.IsExist(request.CityName);
            if (IsExist)
                return TResult<string>.Fail(ApplicationExMsg.CityIsExist);
            clsCity City = _mapper.Map<NewCityCommand,clsCity>(request);

            City.SetCreated(_currentUserService.UserId.ToString());
            string CityID = await _CityRepository.Add(City);

            if (string.IsNullOrEmpty(CityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(CityID);
        }
    }
}
