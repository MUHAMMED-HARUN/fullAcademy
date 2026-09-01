using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Command
{
    public class DeleteAssignmentScopeInfoCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentScopeInfoID { get; set; }
    }
}
