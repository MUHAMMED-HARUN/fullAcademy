using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Authentication.AuthDTO;
using System.Domain.Entities.Address;

namespace System.Application.Services.Address.Address.Command
{
    public class NewAddressCommand : ICommand, IRequest<TResult<string>>
    {
        public string CountryID { get; set; }
        public string CityID{  get; set; }
        public string DistrictID { get; set; }
        public string NeighborhoodID { get; set; }
        public string Notes { get; set; }
    }
}
