using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Address.Country.Command
{
    public class UpdateCountryCommand: ICommand, IRequest<TResult<string>>
    {
        public string CountryID { get; set; }
        public string CountryName {  get; set; }
        public string CountryCode { get; set; }
    }
}
