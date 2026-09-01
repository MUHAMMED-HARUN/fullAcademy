

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Person.Student.Command
{
    public class NewStudentCommand : ICommand, IRequest<TResult<string>>
    {
       public string PersonID { get; set; }
    }
}
