

using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Common;

namespace System.Application.Services.Authentication.RolePermission.Command
{
    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, TResult<bool>>
    {
        IIdentityService _identityService;
        public UpdateRolePermissionsCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<TResult<bool>> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var toRemove = request.Changes.Where(x => x.IsDelete).Distinct().ToList();
            var toAdd = request.Changes.Where(x => !x.IsDelete).Distinct().ToList();
            if (toRemove.Any())
            {
                foreach (var item in toRemove)
                {
                  await  _identityService.DeleteClaims(item);
                }
            }
            if (toAdd.Any())
            {
                foreach (var item in toAdd)
                {
                   await _identityService.AddClaims(item);
                }
            }
            return TResult<bool>.OK(true);
        }
    }
}
