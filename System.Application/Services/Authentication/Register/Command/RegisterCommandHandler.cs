

using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Person;

namespace System.Application.Services.Authentication.Register.Command
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TResult<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IPersonRepository _personRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;

        public RegisterCommandHandler(
            IIdentityService identity,
            IPersonRepository personRepository,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository)
        {
            _identityService = identity;
            _personRepository = personRepository;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
        }
        public async Task<TResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var entityExists = request.UserType switch
            {
                enUserType.Admin =>
                    await _personRepository.GetEntityBy(request.EntityID, enPersonParamType.ID) is not null,
                enUserType.Teacher =>
                    await _teacherRepository.GetBy(request.EntityID, enTeacherParamType.ID) is not null,
                enUserType.Student =>
                    await _studentRepository.GetBy(request.EntityID, enStudentParamType.ID) is not null,
                _ => false,
            };

            if (!entityExists)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            var result = await _identityService.RegisterUserAsync(
                request.Email,
                request.Password,
                request.UserName,
                request.UserType,
                request.EntityID);
            if (!result.IsSuccess)
                return result;

            var roleAssigned = await _identityService.SetUserRole(request.RoleID, result.Value);
            if (!roleAssigned)
            {
                throw new System.Application.Exceptions.ApplicationException(
                    new ApplicationExceptionObj(
                        "REGISTER_ROLE_ASSIGNMENT_FAILED",
                        "تعذر إسناد الدور؛ لم يتم إنشاء الحساب."));
            }

            return TResult<string>.OK(result.Value);
        }
    }
}
