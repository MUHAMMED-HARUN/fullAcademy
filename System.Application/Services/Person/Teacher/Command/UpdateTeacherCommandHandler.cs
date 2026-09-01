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
        private readonly ICurrentUserService _currentUserService;
        public UpdateTeacherCommandHandler(ITeacherRepository TeacherRepository, ICurrentUserService currentUser)
        {
            _TeacherRepository = TeacherRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            clsTeacher Teacher = await _TeacherRepository.GetEntityBy(request.PersonID, enTeacherParamType.PersonID);
            bool IsExist = await _TeacherRepository.IsExistBy(request.PersonID, enTeacherParamType.PersonID);

            if ( Teacher.PersonID.ToString() == request.PersonID && IsExist)
                return TResult<string>.Fail(ApplicationExMsg.PersonIsAlreadyTeacher);

            clsTeacher entity =new clsTeacher(clsUtil.ToGuid(request.PersonID));
            entity.SetID(Teacher.Id);

            entity.SetModified(_currentUserService.UserId.ToString());
            await _TeacherRepository.Update(entity);


            return TResult<string>.OK(string.Empty);

        }
    }
}
