using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;


namespace System.Application.Services.Person.Student.Command
{
    public class NewStudentCommandHandler : IRequestHandler<NewStudentCommand, TResult<string>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICurrentUserService _currentUserService;
        public NewStudentCommandHandler(IStudentRepository studentRepository ,ICurrentUserService currentUser)
        {
            _studentRepository = studentRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(NewStudentCommand request, CancellationToken cancellationToken)
        {
            if ((await _studentRepository.IsStudent(request.PersonID, enStudentParamType.PersonID)) == true)
                return TResult<string>.Fail(ApplicationExMsg.PersonIsAlreadyStudent);

            clsStudent entity = new clsStudent(clsUtil.ToGuid(request.PersonID)); 

            entity.SetCreated(_currentUserService.UserId.ToString());
            string entityID = await _studentRepository.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
