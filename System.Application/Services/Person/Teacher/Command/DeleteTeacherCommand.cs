

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Person.Teacher.Command
{
    public class DeleteTeacherCommand : ICommand, IRequest<TResult<string>>
    {
        public string TeacherID { get; set; }
    }
}
