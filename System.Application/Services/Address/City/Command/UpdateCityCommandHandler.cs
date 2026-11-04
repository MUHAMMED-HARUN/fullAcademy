using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.City.DTO;
using System.Domain.Entities;
using System.Domain.Enums;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.City.Command
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, TResult<string>>
    {
        private readonly ICityRepository _CityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateCityCommandHandler(
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
        public async Task<TResult<string>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var data = await _CityRepository.GetById(request.CityID, enCityParamType.ID);
            if (data==null)
                return TResult<string>.Fail(ApplicationExMsg.CityNotFound);

            var country = await _countryRepository.GetById(request.CountryID, enCountryParamType.ID);
            if (country is null)
                return TResult<string>.Fail(ApplicationExMsg.CountryNotFound);

            clsCity City = _mapper.Map<UpdateCityCommand, clsCity>(request);

            City.SetID(clsUtil.ToGuid(request.CityID));
            City.SetModified( _currentUserService.UserId.ToString());

            await _CityRepository.Update(City);


            return TResult<string>.OK(string.Empty);

            
        }

   
    }
}
