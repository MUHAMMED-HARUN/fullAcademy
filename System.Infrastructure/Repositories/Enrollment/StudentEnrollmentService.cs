using Azure.Core;
using Gridify;
using Gridify.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Enrollment.StudentEnrollment.Query;
using System.Collections.Generic;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Enrollment
{
    public class StudentEnrollmentService : IStudentEnrollmentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsStudentEnrollment> _dbSet;

        public StudentEnrollmentService(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet = _context.Set<clsStudentEnrollment>();
        }

        public async Task<string> Add(clsStudentEnrollment entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StudentEnrollmentInfo> GetBy(string param, enStudentEnrollmentType paramType)
        {
            if (paramType == enStudentEnrollmentType.ID)
            {
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param))
                    .Select(i => new StudentEnrollmentInfo
                    {
                        EnrollmentID = i.Id.ToString(),
                        StudentID = i.StudentID.ToString(),
                        StudentName = i.Student.Person.FirstName + " " + i.Student.Person.FatherName + " " + i.Student.Person.LastName,
                        GroupID = i.GroupID.ToString(),
                        GroupName = i.Group.GroupName,
                        Date = i.Date
                    }).FirstOrDefaultAsync();
            }
            else if (paramType == enStudentEnrollmentType.StudentID)
            {
                return await _dbSet.Where(i => i.StudentID == clsUtil.ToGuid(param))
                    .Select(i => new StudentEnrollmentInfo
                    {
                        EnrollmentID = i.Id.ToString(),
                        StudentID = i.StudentID.ToString(),
                        StudentName = i.Student.Person.FirstName + " " + i.Student.Person.FatherName + " " + i.Student.Person.LastName,
                        GroupID = i.GroupID.ToString(),
                        GroupName = i.Group.GroupName,
                        Date = i.Date
                    }).FirstOrDefaultAsync();
            }
            else // GroupID
            {
                return await _dbSet.Where(i => i.GroupID == clsUtil.ToGuid(param))
                    .Select(i => new StudentEnrollmentInfo
                    {
                        EnrollmentID = i.Id.ToString(),
                        StudentID = i.StudentID.ToString(),
                        StudentName = i.Student.Person.FirstName + " " + i.Student.Person.FatherName + " " + i.Student.Person.LastName,
                        GroupID = i.GroupID.ToString(),
                        GroupName = i.Group.GroupName,
                        Date = i.Date
                    }).FirstOrDefaultAsync();
            }
        }

        public async Task<clsStudentEnrollment> GetEntityBy(string param, enStudentEnrollmentType paramType)
        {
            if (paramType == enStudentEnrollmentType.ID)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
            else if (paramType == enStudentEnrollmentType.StudentID)
                return await _dbSet.Where(i => i.StudentID == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(i => i.GroupID == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
        }

        public async Task<Paging<StudentEnrollmentInfoView>> GetGroupStudentss(GetAllStudentEnrollmentQuery requset, string TeacherID)
        {
            var query = _context.vw_StudentEnrollmentInfo.AsQueryable();

            if (!string.IsNullOrEmpty(TeacherID))
            {
                var teacherGuid = clsUtil.ToGuid(TeacherID);
                var authuraizedGroupids =
                    _context.TeachingAssignments.Where(ta => ta.TeacherID == teacherGuid && ta.IsActive == true).Select(ta=>ta.GroupID);
                query = query.Where(x => authuraizedGroupids.Contains(x.GroupID));

            }
          
            // 4.  ÿ»Ìﬁ «· ’›Ì… Ê«· —ﬁÌ„ (Pagination) »«” Œœ«„ Gridify
            return await query.GridifyAsync(requset);

           

        }

        public async Task<List<StudentEnrollmentInfo>> GetList()
        {
            return await _dbSet.Where(i => i.DeletedAt == null)
                .Select(i => new StudentEnrollmentInfo
                {
                    EnrollmentID = i.Id.ToString(),
                    StudentID = i.StudentID.ToString(),
                    StudentName = i.Student.Person.FirstName + " " + i.Student.Person.FatherName + " " + i.Student.Person.LastName,
                    GroupID = i.GroupID.ToString(),
                    GroupName = i.Group.GroupName,
                    Date = i.Date
                }).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enStudentEnrollmentType paramType)
        {
            if (paramType == enStudentEnrollmentType.ID)
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
            else if (paramType == enStudentEnrollmentType.StudentID)
                return await _dbSet.AnyAsync(i => i.StudentID == clsUtil.ToGuid(param));
            else
                return await _dbSet.AnyAsync(i => i.GroupID == clsUtil.ToGuid(param));
        }

        public async Task<bool> IsStudentEnrolled(string studentID, string groupID)
        {
            return await _dbSet.AnyAsync(i => i.StudentID == clsUtil.ToGuid(studentID) && i.GroupID == clsUtil.ToGuid(groupID));
        }

        public async Task<bool> Update(clsStudentEnrollment entity)
        {
            _dbSet.Update(entity);
            return true;
        }
    }
}
