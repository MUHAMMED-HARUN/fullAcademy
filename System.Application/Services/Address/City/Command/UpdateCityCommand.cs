using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Address.City.Command
{
    public class UpdateCityCommand: ICommand, IRequest<TResult<string>>
    {
        public string CityID { get; set; }
        public string CityName {  get; set; }
        public string CityCode { get; set; }
        public string CountryID{ get; set; }

    }
}
