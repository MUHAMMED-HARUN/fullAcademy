using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Enrollment.StudentEnrollment.Query;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;
using System.Domain.Entities;
using System.Domain.Enums.AcademicPlan;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Enrollment
{
    public sealed class StudentEnrollmentService : IStudentEnrollmentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsStudentEnrollment> _dbSet;
        private readonly ICurrentUserService _currentUser;

        public StudentEnrollmentService(AppDbContext appDbContext, ICurrentUserService currentUser)
        {
            _context = appDbContext;
            _dbSet = _context.Set<clsStudentEnrollment>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsStudentEnrollment entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StudentEnrollmentInfo?> GetBy(string param, enStudentEnrollmentType paramType)
        {
            var query = _dbSet.AsNoTracking()
                .ApplyStudentEnrollmentReadScope(_currentUser, _context);

            if (paramType == enStudentEnrollmentType.ID && Guid.TryParse(param, out var enrollmentID))
                query = query.Where(item => item.Id == enrollmentID);
            else if (paramType == enStudentEnrollmentType.StudentID && Guid.TryParse(param, out var studentID))
                query = query.Where(item => item.StudentID == studentID);
            else if (paramType == enStudentEnrollmentType.GroupID && Guid.TryParse(param, out var groupID))
                query = query.Where(item => item.GroupID == groupID);
            else
                return null;

            return await query.Select(item => new StudentEnrollmentInfo
            {
                EnrollmentID = item.Id.ToString(),
                StudentID = item.StudentID.ToString(),
                StudentName = item.Student.Person.FirstName + " " +
                              item.Student.Person.FatherName + " " +
                              item.Student.Person.LastName,
                GroupID = item.GroupID.ToString(),
                GroupName = item.Group.GroupName,
                StudentAcademicEnrollmentID = item.StudentAcademicEnrollmentID.ToString(),
                AcademicPlanName = item.StudentAcademicEnrollment.AcademicPlan.Name,
                Date = item.Date,
            }).FirstOrDefaultAsync();
        }

        public async Task<clsStudentEnrollment?> GetEntityBy(string param, enStudentEnrollmentType paramType)
        {
            var query = _dbSet.ApplyStudentEnrollmentReadScope(_currentUser, _context);

            if (paramType == enStudentEnrollmentType.ID && Guid.TryParse(param, out var enrollmentID))
                query = query.Where(item => item.Id == enrollmentID);
            else if (paramType == enStudentEnrollmentType.StudentID && Guid.TryParse(param, out var studentID))
                query = query.Where(item => item.StudentID == studentID);
            else if (paramType == enStudentEnrollmentType.GroupID && Guid.TryParse(param, out var groupID))
                query = query.Where(item => item.GroupID == groupID);
            else
                return null;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Paging<StudentEnrollmentInfoView>> GetGroupStudentss(
            GetAllStudentEnrollmentQuery request,
            string teacherID)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var scopedEnrollmentIDs = _dbSet.AsNoTracking()
                .ApplyStudentEnrollmentReadScope(_currentUser, _context)
                .Select(enrollment => enrollment.Id);

            var query = from view in _context.vw_StudentEnrollmentInfo.AsNoTracking()
                        join enrollment in _dbSet.AsNoTracking().ApplyStudentEnrollmentReadScope(_currentUser, _context)
                            on view.EnrollmentID equals enrollment.Id
                        where scopedEnrollmentIDs.Contains(view.EnrollmentID)
                        select new StudentEnrollmentInfoView
                        {
                            EnrollmentID = view.EnrollmentID,
                            StudentID = view.StudentID,
                            StudentName = view.StudentName,
                            GroupID = view.GroupID,
                            GroupName = view.GroupName,
                            Date = view.Date,
                            CanTrack = view.CanTrack,
                            CanNominate = view.CanNominate,
                            CanAddToStage = view.CanAddToStage,
                            StudentAcademicEnrollmentID = enrollment.StudentAcademicEnrollmentID,
                            AcademicPlanName = enrollment.StudentAcademicEnrollment.AcademicPlan.Name,
                        };

            return await query.GridifyAsync(request);
        }

        public async Task<List<StudentEnrollmentInfo>> GetList()
        {
            var query = _dbSet.AsNoTracking()
                .ApplyStudentEnrollmentReadScope(_currentUser, _context);

            return await query.Select(item => new StudentEnrollmentInfo
            {
                EnrollmentID = item.Id.ToString(),
                StudentID = item.StudentID.ToString(),
                StudentName = item.Student.Person.FirstName + " " +
                              item.Student.Person.FatherName + " " +
                              item.Student.Person.LastName,
                GroupID = item.GroupID.ToString(),
                GroupName = item.Group.GroupName,
                StudentAcademicEnrollmentID = item.StudentAcademicEnrollmentID.ToString(),
                AcademicPlanName = item.StudentAcademicEnrollment.AcademicPlan.Name,
                Date = item.Date,
            }).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enStudentEnrollmentType paramType)
        {
            var query = _dbSet.AsNoTracking()
                .ApplyStudentEnrollmentReadScope(_currentUser, _context);

            if (paramType == enStudentEnrollmentType.ID && Guid.TryParse(param, out var enrollmentID))
                return await query.AnyAsync(item => item.Id == enrollmentID);
            if (paramType == enStudentEnrollmentType.StudentID && Guid.TryParse(param, out var studentID))
                return await query.AnyAsync(item => item.StudentID == studentID);
            if (paramType == enStudentEnrollmentType.GroupID && Guid.TryParse(param, out var groupID))
                return await query.AnyAsync(item => item.GroupID == groupID);

            return false;
        }

        public async Task<bool> IsStudentEnrolled(string studentID, string groupID)
        {
            if (!Guid.TryParse(studentID, out var studentGuid) ||
                !Guid.TryParse(groupID, out var groupGuid))
                return false;

            return await _dbSet.AsNoTracking()
                .ApplyStudentEnrollmentReadScope(_currentUser, _context)
                .AnyAsync(item =>
                    item.StudentID == studentGuid &&
                    item.GroupID == groupGuid);
        }

        public async Task<StudentAcademicEnrollmentMembershipInfo?> GetEligibleAcademicEnrollmentForGroup(
            Guid studentAcademicEnrollmentID,
            Guid groupID,
            DateOnly date)
        {
            if (studentAcademicEnrollmentID == Guid.Empty || groupID == Guid.Empty || date == default)
                return null;

            var groupClass = await _context.Groups.AsNoTracking()
                .Where(group => group.Id == groupID)
                .Select(group => new
                {
                    group.InstituteClassID,
                    group.InstituteClass.ClassID,
                    group.InstituteClass.Class.ProgramID,
                })
                .FirstOrDefaultAsync();
            if (groupClass == null)
                return null;

            var enrollment = await _context.StudentAcademicEnrollments.AsNoTracking()
                .ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context)
                .Where(item => item.Id == studentAcademicEnrollmentID &&
                               item.Status == enStudentAcademicEnrollmentStatus.Active &&
                               item.ValidFrom <= date && (!item.ValidTo.HasValue || item.ValidTo >= date))
                .Select(item => new
                {
                    item.Id,
                    item.StudentID,
                    StudentName = item.Student.Person.FirstName + " " + item.Student.Person.FatherName + " " + item.Student.Person.LastName,
                    item.AcademicPlan.Name,
                    item.AcademicPlan.Version,
                    item.AcademicPlan.ScopeType,
                    item.AcademicPlan.ProgramID,
                    item.AcademicPlan.InstituteClassID,
                    item.AcademicPlan.StageID,
                    item.AcademicPlan.StageInfoID,
                    item.ValidFrom,
                    item.ValidTo,
                })
                .FirstOrDefaultAsync();
            if (enrollment == null)
                return null;

            var groupMatchesPlan = enrollment.ScopeType switch
            {
                enAcademicPlanScopeType.Program => enrollment.ProgramID == groupClass.ProgramID,
                enAcademicPlanScopeType.InstituteClass => enrollment.InstituteClassID == groupClass.InstituteClassID,
                enAcademicPlanScopeType.Stage => await _context.Stages.AsNoTracking()
                    .AnyAsync(stage => stage.Id == enrollment.StageID && stage.ClassID == groupClass.ClassID),
                enAcademicPlanScopeType.StageInfo => await _context.StageInfos.AsNoTracking()
                    .AnyAsync(info => info.Id == enrollment.StageInfoID &&
                                      _context.Stages.Any(stage => stage.Id == info.StageID && stage.ClassID == groupClass.ClassID)),
                _ => false,
            };
            if (!groupMatchesPlan)
                return null;

            return new StudentAcademicEnrollmentMembershipInfo
            {
                StudentAcademicEnrollmentID = enrollment.Id.ToString(),
                StudentID = enrollment.StudentID.ToString(),
                StudentName = enrollment.StudentName,
                AcademicPlanName = enrollment.Name,
                AcademicPlanVersion = enrollment.Version,
                ValidFrom = enrollment.ValidFrom,
                ValidTo = enrollment.ValidTo,
            };
        }

        public Task<bool> IsAcademicEnrollmentAlreadyInGroup(Guid studentAcademicEnrollmentID, Guid groupID) =>
            _dbSet.AsNoTracking().AnyAsync(item =>
                item.StudentAcademicEnrollmentID == studentAcademicEnrollmentID && item.GroupID == groupID);

        public Task<bool> Update(clsStudentEnrollment entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(true);
        }
    }
}
