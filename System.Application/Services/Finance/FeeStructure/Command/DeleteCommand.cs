using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Finance.FeeStructure.Command
{
    public class DeleteFeeStructureCommand : ICommand, IRequest<TResult<string>>
    {
        public string FeeStructureID { get; set; }
    }
}
