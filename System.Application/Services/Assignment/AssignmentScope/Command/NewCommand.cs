using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScope.Command
{
    public class NewAssignmentScopeCommand : ICommand, IRequest<TResult<string>>
    {
        public string Name { get; set; }
        public string? Notes { get; set; }
        public string StageID { get; set; }
        public string? PrevAssignmentScopeID { get; set; }
    }
}
