using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;


namespace System.Application.Services.Person.Student.Command
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, TResult<string>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateStudentCommandHandler(
            IStudentRepository studentRepository,
            IPersonRepository personRepository,
            ICurrentUserService currentUser)
        {
            _studentRepository = studentRepository;
            _personRepository = personRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            clsStudent student = await _studentRepository.GetEntityBy(request.StudentID, enStudentParamType.ID);
            if (student == null)
                return TResult<string>.Fail(ApplicationExMsg.FailToFindStudent);

            var person = await _personRepository.GetEntityBy(
                request.PersonID,
                enPersonParamType.ID);
            if (person is null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            Guid personID = clsUtil.ToGuid(request.PersonID);
            if (student.PersonID != personID &&
                await _studentRepository.IsExistBy(request.PersonID, enStudentParamType.PersonID))
                return TResult<string>.Fail(ApplicationExMsg.PersonIsAlreadyStudent);

            student.SetPersonID(personID);
            student.SetModified(_currentUserService.UserId.ToString());
            await _studentRepository.Update(student);
            return TResult<string>.OK(string.Empty);

        }
    }
}
