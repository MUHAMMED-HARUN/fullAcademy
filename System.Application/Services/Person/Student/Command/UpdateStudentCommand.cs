using MediatR;
using System.Application.Abstractions;
using System.Application.Common;


namespace System.Application.Services.Person.Student.Command
{
    public class UpdateStudentCommand : ICommand, IRequest<TResult<string>>
    {
       public string StudentID { get; set; }
        public string PersonID { get; set; }

    }
}
