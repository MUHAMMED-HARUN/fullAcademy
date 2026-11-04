

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Address.Command;
using System.Domain.Entities.Person;

namespace System.Application.Services.Person.Person.Command
{
    public class UpdatePersonCommand : ICommand, ITransaction, IRequest<TResult<string>>
    {
        public string PersonID { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string LastName { get; set; }
        public string MotherName { get; set; }
        public string MotherLastName { get; set; }
        public string NationalNumber { get; set; }
        public bool Gendor { get; set; }
        public DateOnly BirthDate { get; set; }
        public string PlaceOfBirthID { get; set; }
        public UpdateAddressCommand Address { get; set; }
    }
}
