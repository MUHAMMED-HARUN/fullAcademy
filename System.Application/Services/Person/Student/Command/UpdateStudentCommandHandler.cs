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
        private readonly ICurrentUserService _currentUserService;
        public UpdateStudentCommandHandler(IStudentRepository studentRepository, ICurrentUserService currentUser)
        {
            _studentRepository = studentRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            clsStudent student = await _studentRepository.GetEntityBy(request.PersonID, enStudentParamType.PersonID);
            bool IsExist = await _studentRepository.IsExistBy(request.PersonID, enStudentParamType.PersonID);

            if ( student.PersonID.ToString() == request.PersonID && IsExist)
                return TResult<string>.Fail(ApplicationExMsg.PersonIsAlreadyStudent);

            clsStudent entity =new clsStudent(clsUtil.ToGuid(request.PersonID));
            entity.SetID(student.Id);

            entity.SetModified(_currentUserService.UserId.ToString());
            await _studentRepository.Update(entity);


            return TResult<string>.OK(string.Empty);

        }
    }
}
