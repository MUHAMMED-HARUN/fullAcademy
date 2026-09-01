

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Person.Person.Command
{
    public class DeletePersonCommand : ICommand, IRequest<TResult<string>>
    {
        public string PersonID { get; set; }
    }
}
