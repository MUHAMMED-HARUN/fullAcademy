using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.InstituteClass.DTO;

namespace System.Application.Services.Institute.InstituteClass.Command
{
    public class NewInstituteClassByRangeCommand : ICommand, IRequest<TResult<InstituteClassBulkResult>>
    {
        public List<string> ClassIDs { get; set; } = new();
        public string InstituteID { get; set; } = string.Empty;
    }
}
