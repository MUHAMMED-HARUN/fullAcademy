
using AutoMapper;
using System.Application.Common;
using System.Application.Services.Address.Country.Command;
using System.Application.Services.Address.District.Command;
using System.Domain.Entities;

namespace System.Application.Services.Address.District.Mapper
{
    public class DistrictProfile :Profile
    {
        public DistrictProfile()
        {
            CreateMap<NewDistrictCommand, clsDistrict>()
                  .ConstructUsing(src => new clsDistrict(
                 clsUtil.ToGuid(src.CityID), src.DistrictName
                  )); 

            CreateMap<UpdateDistrictCommand, clsDistrict>()
                  .ConstructUsing(src => new clsDistrict(
                 clsUtil.ToGuid(src.CityID), src.DistrictName   
                  ));
        }
    }
}
