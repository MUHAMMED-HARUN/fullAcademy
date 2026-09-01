
using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Authentication.AuthDTO;
namespace System.Application.Services.Authentication.Login.Command
{
    public class LoginCommand:ICommand,IRequest<TResult<AuthDTOInfo>>
    {
        public string Email{ get; set; }
        public string Password { get; set; }
    }
}
