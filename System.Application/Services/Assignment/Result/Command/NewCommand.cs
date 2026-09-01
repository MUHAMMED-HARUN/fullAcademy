using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.Result.Command
{
    public class NewResultCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentID { get; set; }
        public string AssignmentScopeInfoID { get; set; }
        public double Score { get; set; }
        public string? Notes { get; set; }
    }
}
