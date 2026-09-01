using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Address.Neighborhood.Command
{
    public class UpdateNeighborhoodCommand : ICommand, IRequest<TResult<string>>
    {
        public string NeighborhoodID{ get; set; }
        public string NeighborhoodName { get; set; }
        public string DistrictID { get; set; }
    }
}
