using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Entities;

namespace System.Application.Services.Address.Neighborhood.Command
{
    public class NewNeighborhoodCommand : ICommand, IRequest<TResult<string>>
    {
        public string DistrictID{  get; set; }
        public string NeighborhoodName{ get; set; }
    }
}
