
using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Authentication.AuthDTO;
using System.Application.Services.Authentication.Login.DTO;
namespace System.Application.Services.Authentication.Login.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TResult<AuthDTOInfo>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public LoginCommandHandler(IIdentityService identityService,IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<TResult<AuthDTOInfo>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
       

            TResult <LoginDto> result = await _identityService.LoginAsync(request.Email, request.Password);

            if (!result.IsSuccess || result.Value is null)
                return TResult<AuthDTOInfo>.Fail(result.Error ?? "البريد الإلكتروني أو كلمة المرور غير صحيحة");

            AuthDTOInfo dto = _mapper.Map<AuthDTOInfo>(result.Value);


            return TResult<AuthDTOInfo>.OK(dto);
        }
    } 
    }
