using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.Session.Command
{
    public class UpdateSessionCommand : ICommand, IRequest<TResult<string>>
    {
        public string SessionID { get; set; } = string.Empty;
        public string SessionName { get; set; } = string.Empty;
        public enSessionType SessionType { get; set; }
        public string? SessionNote { get; set; }
        public string? ParentSessionID { get; set; }
    }
}
