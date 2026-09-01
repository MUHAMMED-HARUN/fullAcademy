

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.Institute.Command
{
    public class DeleteInstituteCommand : ICommand, IRequest<TResult<string>>
    {
        public string InstituteID { get; set; }
    }
}
