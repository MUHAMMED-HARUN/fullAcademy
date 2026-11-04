using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;


namespace System.Application.Services.Person.Teacher.Command
{
    public class NewTeacherCommandHandler : IRequestHandler<NewTeacherCommand, TResult<string>>
    {
        private readonly ITeacherRepository _TeacherRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentUserService _currentUserService;
        public NewTeacherCommandHandler(
            ITeacherRepository TeacherRepository,
            IPersonRepository personRepository,
            ICurrentUserService currentUser)
        {
            _TeacherRepository = TeacherRepository;
            _personRepository = personRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(NewTeacherCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetEntityBy(
                request.PersonID,
                enPersonParamType.ID);
            if (person is null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            if ((await _TeacherRepository.IsTeacher(request.PersonID, enTeacherParamType.PersonID)) == true)
                return TResult<string>.Fail(ApplicationExMsg.PersonIsAlreadyTeacher);

            clsTeacher entity = new clsTeacher(clsUtil.ToGuid(request.PersonID)); 

            entity.SetCreated(_currentUserService.UserId.ToString());
            string entityID = await _TeacherRepository.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
