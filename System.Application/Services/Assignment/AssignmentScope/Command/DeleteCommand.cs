using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScope.Command
{
    public class DeleteAssignmentScopeCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentScopeID { get; set; }
    }
}
