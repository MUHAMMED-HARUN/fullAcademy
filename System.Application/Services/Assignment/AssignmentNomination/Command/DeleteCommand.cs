using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentNomination.Command
{
    public class DeleteAssignmentNominationCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentNominationID { get; set; }
    }
}
