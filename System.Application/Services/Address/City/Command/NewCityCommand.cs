using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Authentication.AuthDTO;

namespace System.Application.Services.Address.City.Command
{
    public class NewCityCommand: ICommand, IRequest<TResult<string>>
    {
        public string CityName {  get; set; }
        public string CityCode { get; set; }
        public string CountryID { get; set; }
    }
}
