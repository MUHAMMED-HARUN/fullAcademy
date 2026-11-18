using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class DeleteAssignmentCommand : ITransaction, IRequest<TResult<string>>
    {
        public string AssignmentID { get; set; } = string.Empty;
    }
}
