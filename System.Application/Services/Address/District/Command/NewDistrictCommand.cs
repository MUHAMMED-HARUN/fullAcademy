using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Address.District.Command
{
    public class NewDistrictCommand: ICommand, IRequest<TResult<string>>
    {
        public string DistrictName {  get; set; }
        public string CityID { get; set; }
    }
}
