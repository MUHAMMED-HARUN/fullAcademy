using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;


namespace System.Application.Services.Person.Teacher.Command
{
    public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, TResult<string>>
    {
        private readonly ITeacherRepository _TeacherRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateTeacherCommandHandler(
            ITeacherRepository TeacherRepository,
            IPersonRepository personRepository,
            ICurrentUserService currentUser)
        {
            _TeacherRepository = TeacherRepository;
            _personRepository = personRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            clsTeacher teacher = await _TeacherRepository.GetEntityBy(request.TeacherID, enTeacherParamType.ID);
            if (teacher == null)
                return TResult<string>.Fail(ApplicationExMsg.FailToFindTeacher);

            var person = await _personRepository.GetEntityBy(
                request.PersonID,
                enPersonParamType.ID);
            if (person is null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            Guid personID = clsUtil.ToGuid(request.PersonID);
            if (teacher.PersonID != personID &&
                await _TeacherRepository.IsExistBy(request.PersonID, enTeacherParamType.PersonID))
                return TResult<string>.Fail(ApplicationExMsg.PersonIsAlreadyTeacher);

            teacher.SetPersonID(personID);
            teacher.SetModified(_currentUserService.UserId.ToString());
            await _TeacherRepository.Update(teacher);
            return TResult<string>.OK(string.Empty);

        }
    }
}
