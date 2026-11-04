

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Progress.Session.Command
{
    public class DeleteSessionCommand : ICommand, IRequest<TResult<string>>
    {
        public string SessionID { get; set; } = string.Empty;
    }
}
