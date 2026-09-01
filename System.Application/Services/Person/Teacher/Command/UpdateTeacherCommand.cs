using MediatR;
using System.Application.Abstractions;
using System.Application.Common;


namespace System.Application.Services.Person.Teacher.Command
{
    public class UpdateTeacherCommand : ICommand, IRequest<TResult<string>>
    {
       public string TeacherID { get; set; }
        public string PersonID { get; set; }

    }
}
