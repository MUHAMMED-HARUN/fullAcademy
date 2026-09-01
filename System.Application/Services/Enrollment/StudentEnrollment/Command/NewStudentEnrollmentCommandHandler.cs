using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentEnrollment.Command;
using System.Domain.Entities;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class NewStudentEnrollmentCommandHandler : IRequestHandler<NewStudentEnrollmentCommand, TResult<string>>
    {
        private readonly IStudentEnrollmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewStudentEnrollmentCommandHandler(IStudentEnrollmentRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewStudentEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.IsStudentEnrolled(request.StudentID, request.GroupID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAlreadyEnrolledInGroup);

            clsStudentEnrollment entity = _mapper.Map<NewStudentEnrollmentCommand, clsStudentEnrollment>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
