using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.Result.Command
{
    public class UpdateResultCommand : ICommand, IRequest<TResult<string>>
    {
        public string ResultID { get; set; }
        public string AssignmentID { get; set; }
        public string AssignmentScopeInfoID { get; set; }
        public double Score { get; set; }
        public string? Notes { get; set; }
    }
}
