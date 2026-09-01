

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
namespace System.Application.Services.Institute.Class.Command
{
    public class UpdateClassCommand : ICommand, IRequest<TResult<string>>
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public int ClassLevel { get; set; }
        public string ClassNotes { get; set; }
        public string ProgramID{ get; set; }
    }
}
