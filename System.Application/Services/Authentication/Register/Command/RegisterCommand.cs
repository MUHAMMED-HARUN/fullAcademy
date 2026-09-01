using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;

namespace System.Application.Services.Authentication.Register.Command
{
    public class RegisterCommand: ITransaction, IRequest<TResult<string>>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
         public string Password { get; set; }
        public string RoleID { get; set; }
        public enUserType UserType { get; set; }
        public string EntityID { get; set; }
    }
}
