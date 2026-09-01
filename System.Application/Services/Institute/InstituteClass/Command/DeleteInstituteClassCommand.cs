

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.InstituteClass.Command
{
    public class DeleteInstituteClassCommand : ICommand, IRequest<TResult<string>>
    {
        public string InstituteClassID { get; set; }
    }
}
