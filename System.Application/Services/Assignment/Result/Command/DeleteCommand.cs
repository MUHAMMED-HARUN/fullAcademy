using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.Result.Command
{
    public class DeleteResultCommand : ICommand, IRequest<TResult<string>>
    {
        public string ResultID { get; set; }
    }
}
