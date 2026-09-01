

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.Group.Command
{
    public class DeleteGroupCommand : ICommand, IRequest<TResult<string>>
    {
        public string GroupID { get; set; }
    }
}
