using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.Result.Command
{
    public class NewResultCommand : ITransaction, IRequest<TResult<string>>
    {
        public string AssignmentID { get; set; } = string.Empty;
        public string AssignmentScopeInfoID { get; set; } = string.Empty;
        public double Score { get; set; }
        public string? Notes { get; set; }
    }
}
