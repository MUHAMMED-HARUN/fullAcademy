

using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;

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
            var uniqueChanges = request.Changes
                .GroupBy(change => new
                {
                    change.RoleId,
                    PermissionName = change.PermissionName.Trim(),
                })
                .Select(group => group.Last())
                .ToList();

            foreach (var change in uniqueChanges.Where(change => change.IsDelete))
            {
                if (!await _identityService.DeleteClaims(change))
                    throw FailedToApplyChange();
            }

            foreach (var change in uniqueChanges.Where(change => !change.IsDelete))
            {
                if (!await _identityService.AddClaims(change))
                    throw FailedToApplyChange();
            }

            return TResult<bool>.OK(true);
        }

        private static System.Application.Exceptions.ApplicationException FailedToApplyChange() =>
            new(new ApplicationExceptionObj(
                "ROLE_PERMISSION_UPDATE_FAILED",
                "تعذر تطبيق تغييرات الصلاحيات؛ لم يتم حفظ أي تغيير."));
    }
}
