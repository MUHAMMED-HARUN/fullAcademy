using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Command
{
    public class NewAssignmentScopeInfoCommand : ICommand, IRequest<TResult<string>>
    {
        public short MaxScore { get; set; }
        public short MinPassScore { get; set; }
        public float Weight { get; set; }
        public string StageInfoID { get; set; }
        public string AssignmentScopeID { get; set; }
    }
}
