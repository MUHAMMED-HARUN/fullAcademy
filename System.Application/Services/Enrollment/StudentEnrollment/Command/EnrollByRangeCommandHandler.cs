using AutoMapper;
using MediatR;
using System;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentEnrollment.Command;
using System.Collections.Generic;
using System.Domain.Entities;
using System.Linq;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class EnrollByRangeCommandHandler : IRequestHandler<EnrollByRangeCommand, TResult<List<string>>>
    {
        private readonly IStudentEnrollmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public EnrollByRangeCommandHandler(IStudentEnrollmentRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<List<string>>> Handle(EnrollByRangeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.GroupID))
                return TResult<List<string>>.Fail("GroupID is required");

            if (request.StudentIDs == null || request.StudentIDs.Count == 0)
                return TResult<List<string>>.Fail("StudentIDs is required");

            var enrolledIds = new List<string>();
            var userId = _currentUserService.UserId.ToString();

            foreach (var studentID in request.StudentIDs.Distinct())
            {
                if (await _repo.IsStudentEnrolled(studentID, request.GroupID))
                    continue;

                var entity = new clsStudentEnrollment(
                    clsUtil.ToGuid(studentID),
                    clsUtil.ToGuid(request.GroupID),
                    request.Date == default ? DateOnly.FromDateTime(DateTime.Now) : request.Date
                );
                entity.SetCreated(userId);

                string entityID = await _repo.Add(entity);
                if (!string.IsNullOrEmpty(entityID))
                    enrolledIds.Add(entityID);
            }

            if (enrolledIds.Count == 0)
                return TResult<List<string>>.Fail("No students enrolled");

            return TResult<List<string>>.OK(enrolledIds);
        }
    }
}