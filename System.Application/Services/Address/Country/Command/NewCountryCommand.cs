using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Address.Country.Command
{
    public class NewCountryCommand: ICommand, IRequest<TResult<string>>
    {
        public string CountryName {  get; set; }
        public string CountryCode { get; set; }
    }
}
