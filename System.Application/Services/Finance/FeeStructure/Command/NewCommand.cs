using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.FeeStructure.Command
{
    public class NewFeeStructureCommand : ICommand, IRequest<TResult<string>>
    {
        public string Name { get; set; }
        public enFeeType FeeType { get; set; }
        public decimal Amount { get; set; }
        public string? ProgramID { get; set; }
        public string? ClassID { get; set; }
        public string? StageID { get; set; }
        public string? StageInfoID { get; set; }
        public string? Notes { get; set; }
    }
}
