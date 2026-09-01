using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Entities.Permissions;

namespace System.Application.Services.Address.Country.Command
{
    public class NewCountryCommandHandler : IRequestHandler<NewCountryCommand, TResult<string>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewCountryCommandHandler(ICountryRepository countryRepository,ICurrentUserService currentUserService,IMapper mapper)
        {
            _countryRepository = countryRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewCountryCommand request, CancellationToken cancellationToken)
        {
            bool IsExist = await _countryRepository.IsExist(request.CountryName);
            if (IsExist)
                return TResult<string>.Fail(ApplicationExMsg.CountryIsExist);
            clsCountry Country = _mapper.Map<NewCountryCommand,clsCountry>(request);

            Country.SetCreated(_currentUserService.UserId.ToString());
            string countyID = await _countryRepository.Add(Country);

            if (string.IsNullOrEmpty(countyID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(countyID);
        }
    }
}
