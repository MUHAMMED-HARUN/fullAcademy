using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.InstituteClass.Command
{
    public class UpdateInstituteClassCommand : ICommand, IRequest<TResult<string>>
    {
        public string InstituteClassID { get; set; }
        public string ClassID{ get; set; }
        public string InstituteID{ get; set; }
    }
}
