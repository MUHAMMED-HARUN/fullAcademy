using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Address.District.Command
{
    public class UpdateDistrictCommand: ICommand, IRequest<TResult<string>>
    {
        public string DistrictID { get; set; }
        public string DistrictName {  get; set; }
        public string CityID { get; set; }
    }
}
