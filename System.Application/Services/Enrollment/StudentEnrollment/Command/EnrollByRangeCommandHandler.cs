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

            if (request.StudentAcademicEnrollmentIDs == null || request.StudentAcademicEnrollmentIDs.Count == 0)
                return TResult<List<string>>.Fail("StudentAcademicEnrollmentIDs is required");

            var enrolledIds = new List<string>();
            if (_currentUserService.UserId is not Guid userID)
                return TResult<List<string>>.Fail("المستخدم الحالي غير صالح");

            var userId = userID.ToString();
            var groupID = clsUtil.ToGuid(request.GroupID);
            var date = request.Date == default ? DateOnly.FromDateTime(DateTime.Now) : request.Date;
            foreach (var academicEnrollmentValue in request.StudentAcademicEnrollmentIDs.Distinct())
            {
                if (!Guid.TryParse(academicEnrollmentValue, out var academicEnrollmentID) || academicEnrollmentID == Guid.Empty)
                    continue;
                var enrollmentContext = await _repo.GetEligibleAcademicEnrollmentForGroup(academicEnrollmentID, groupID, date);
                if (enrollmentContext == null || await _repo.IsAcademicEnrollmentAlreadyInGroup(academicEnrollmentID, groupID))
                    continue;

                var entity = new clsStudentEnrollment(
                    clsUtil.ToGuid(enrollmentContext.StudentID),
                    groupID,
                    academicEnrollmentID,
                    date
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
