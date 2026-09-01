
using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Authentication.Role.DTO;

namespace System.Application.Services.Authentication.Role.Qurery
{
    public class GetRoleQueryHandler : IRequestHandler<GetRolesQuery, TResult<List<RoleInfo>>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public GetRoleQueryHandler(IIdentityService identity,IMapper mapper)
        {
            _identityService = identity;
            _mapper = mapper;
        }
        public async Task<TResult<List<RoleInfo>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            List<RoleInfo> result= await _identityService.getRoles();
            return TResult<List<RoleInfo>>.OK(result);
        }
    }
}
