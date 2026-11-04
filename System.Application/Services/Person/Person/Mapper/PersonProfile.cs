
using AutoMapper;
using System.Application.Common;
using System.Application.Services.Address.City.Command;
using System.Application.Services.Address.Country.Command;
using System.Application.Services.Person.Person.Command;
using System.Domain.Entities;
using System.Domain.Entities.Person;

namespace System.Application.Services.Person.Person.Mapper
{
    public class PersonProfile :Profile
    {
        public PersonProfile()
        {
            CreateMap<NewPersonCommand, clsPerson>()
                  .ConstructUsing(src => new clsPerson(src.FirstName, src.FatherName, src.LastName, src.MotherName,
                  src.MotherLastName, src.NationalNumber, src.Gendor, src.BirthDate, clsUtil.ToGuid(src.PlaceOfBirthID),
                  Guid.Empty)
                  ); 

            CreateMap<UpdatePersonCommand, clsPerson>()
                .ConstructUsing(src => new clsPerson(src.FirstName, src.FatherName, src.LastName, src.MotherName,
                  src.MotherLastName, src.NationalNumber, src.Gendor, src.BirthDate, clsUtil.ToGuid(src.PlaceOfBirthID),
                  Guid.Empty)
                  );


        }
    }
}
