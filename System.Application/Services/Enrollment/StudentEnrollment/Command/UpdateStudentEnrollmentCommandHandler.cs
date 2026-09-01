using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class UpdateStudentEnrollmentCommandHandler : IRequestHandler<UpdateStudentEnrollmentCommand, TResult<string>>
    {
        private readonly IStudentEnrollmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateStudentEnrollmentCommandHandler(IStudentEnrollmentRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateStudentEnrollmentCommand request, CancellationToken cancellationToken)
        {
            clsStudentEnrollment entity = await _repo.GetEntityBy(request.EnrollmentID, enStudentEnrollmentType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.StudentEnrollmentNotFound);

            if ((await _repo.IsStudentEnrolled(request.StudentID, request.GroupID)) && 
                (entity.StudentID != clsUtil.ToGuid(request.StudentID) || entity.GroupID != clsUtil.ToGuid(request.GroupID)))
            {
                return TResult<string>.Fail(ApplicationExMsg.StudentAlreadyEnrolledInGroup);
            }

            _mapper.Map<UpdateStudentEnrollmentCommand, clsStudentEnrollment>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
