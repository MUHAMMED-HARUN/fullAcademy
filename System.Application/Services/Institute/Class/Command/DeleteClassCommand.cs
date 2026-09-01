

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.Class.Command
{
    public class DeleteClassCommand : ICommand, IRequest<TResult<string>>
    {
        public string ClassID { get; set; }
    }
}
