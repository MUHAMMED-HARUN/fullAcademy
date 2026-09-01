

using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Authentication.Register.Command
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TResult<string>>
    {
        private readonly IIdentityService _identityService;
        public RegisterCommandHandler(IIdentityService identity)
        {
            _identityService = identity;
        }
        public async Task<TResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterUserAsync(
                 request.Email,
                 request.Password,
                 request.UserName,
                 request.UserType,
                 request.EntityID);
            if(result.IsSuccess)
            {
                var AddRole =await _identityService.SetUserRole(request.RoleID, result.Value);
                return TResult<string>.OK(result.Value); 
            }
            
            return TResult<string>.Fail("error");
        }
    }
}
