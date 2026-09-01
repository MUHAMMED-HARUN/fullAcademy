
using AutoMapper;
using System.Application.Common;
using System.Application.Services.Address.City.Command;
using System.Application.Services.Address.Country.Command;
using System.Domain.Entities;

namespace System.Application.Services.Address.City.Mapper
{
    public class CityProfile :Profile
    {
        public CityProfile()
        {
            CreateMap<NewCityCommand, clsCity>()
                  .ConstructUsing(src => new clsCity(
                 clsUtil.ToGuid(src.CountryID), src.CityName,src.CityCode
                  )); 

            CreateMap<UpdateCityCommand, clsCity>()
                  .ConstructUsing(src => new clsCity(
                 clsUtil.ToGuid(src.CountryID), src.CityName, src.CityCode
                  ));
        }
    }
}
