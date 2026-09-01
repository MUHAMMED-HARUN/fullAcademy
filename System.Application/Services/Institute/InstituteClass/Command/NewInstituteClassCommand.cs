using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.InstituteClass.Command
{
    public class NewInstituteClassCommand : ICommand, IRequest<TResult<string>>
    {
        public string ClassID { get; set; }
        public string InstituteID { get; set; }
    }
}
