using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.Class.Command
{
    public class NewClassCommand : ICommand, IRequest<TResult<string>>
    {
        public string ClassName { get; set; }
        public string ClassNotes { get; set; }
        public string ProgramID { get; set; }
        public int ClassLevel { get; set; }
    }
}
