using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.Program.Command
{
    public class NewProgramCommand : ICommand, IRequest<TResult<string>>
    {
        public string ProgramName { get; set; }
        public string ProgramNote { get; set; }
    }
}
