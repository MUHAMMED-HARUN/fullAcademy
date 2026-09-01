

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Person.Teacher.Command
{
    public class NewTeacherCommand : ICommand, IRequest<TResult<string>>
    {
       public string PersonID { get; set; }
    }
}
