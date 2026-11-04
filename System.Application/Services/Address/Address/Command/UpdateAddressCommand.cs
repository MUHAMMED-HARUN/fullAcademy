using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Address.Address.Command
{
    public class UpdateAddressCommand : ICommand, IRequest<TResult<string>>
    {
        public string AddressID { get; set; }
        public string CountryID { get; set; }
        public string CityID { get; set; }
        public string DistrictID { get; set; }
        public string NeighborhoodID { get; set; }
        public string? Notes { get; set; }

    }
}
