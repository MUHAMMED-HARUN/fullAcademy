using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Application.Services.Enrollment.TeachingAssignment.Query;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Enrollment
{
    public class TeachingAssignmentService : ITeachingAssignmentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsTeachingAssignment> _dbSet;

        public TeachingAssignmentService(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet = _context.Set<clsTeachingAssignment>();
        }

        public async Task<string> Add(clsTeachingAssignment entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<TeachingAssignmentInfo> GetBy(string param, enTeachingAssignmentType paramType)
        {
            if (paramType == enTeachingAssignmentType.ID)
            {
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param))
                    .Select(i => new TeachingAssignmentInfo
                    {
                        EnrollmentID = i.Id.ToString(),
                        TeacherID = i.TeacherID.ToString(),
                        TeacherName = i.Teacher.Person.FirstName + " " + i.Teacher.Person.FatherName + " " + i.Teacher.Person.LastName,
                        GroupID = i.GroupID.ToString(),
                        GroupName = i.Group.GroupName,
                        SubjectID = i.SubjectID.ToString(),
                        SubjectName = i.Subject.Name,
                        EnrollmentDate = i.Date
                    }).FirstOrDefaultAsync();
            }
            else if (paramType == enTeachingAssignmentType.TeacherID)
            {
                return await _dbSet.Where(i => i.TeacherID == clsUtil.ToGuid(param))
                     .Select(i => new TeachingAssignmentInfo
                     {
                         EnrollmentID = i.Id.ToString(),
                         TeacherID = i.TeacherID.ToString(),
                         TeacherName = i.Teacher.Person.FirstName + " " + i.Teacher.Person.FatherName + " " + i.Teacher.Person.LastName,
                         GroupID = i.GroupID.ToString(),
                         GroupName = i.Group.GroupName,
                         SubjectID = i.SubjectID.ToString(),
                         SubjectName = i.Subject.Name,
                         EnrollmentDate = i.Date
                     }).FirstOrDefaultAsync();
            }
            else if (paramType == enTeachingAssignmentType.GroupID) // GroupID
            {
                return await _dbSet.Where(i => i.GroupID == clsUtil.ToGuid(param))
                     .Select(i => new TeachingAssignmentInfo
                     {
                         EnrollmentID = i.Id.ToString(),
                         TeacherID = i.TeacherID.ToString(),
                         TeacherName = i.Teacher.Person.FirstName + " " + i.Teacher.Person.FatherName + " " + i.Teacher.Person.LastName,
                         GroupID = i.GroupID.ToString(),
                         GroupName = i.Group.GroupName,
                         SubjectID = i.SubjectID.ToString(),
                         SubjectName = i.Subject.Name,
                         EnrollmentDate = i.Date
                     }).FirstOrDefaultAsync();
            }
            else
            {
             return await _dbSet.Where(i => i.SubjectID == clsUtil.ToGuid(param))
                                 .Select(i => new TeachingAssignmentInfo
                                 {
                                     EnrollmentID = i.Id.ToString(),
                                     TeacherID = i.TeacherID.ToString(),
                                     TeacherName = i.Teacher.Person.FirstName + " " + i.Teacher.Person.FatherName + " " + i.Teacher.Person.LastName,
                                     GroupID = i.GroupID.ToString(),
                                     GroupName = i.Group.GroupName,
                                     SubjectID = i.SubjectID.ToString(),
                                     SubjectName = i.Subject.Name,
                                     EnrollmentDate = i.Date
                                 }).FirstOrDefaultAsync();
            }
        }

        public async Task<clsTeachingAssignment> GetEntityBy(string param, enTeachingAssignmentType paramType)
        {
            if (paramType == enTeachingAssignmentType.ID)
            {
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param))
                   .FirstOrDefaultAsync();
            }
            else if (paramType == enTeachingAssignmentType.TeacherID)
            {
                return await _dbSet.Where(i => i.TeacherID == clsUtil.ToGuid(param))
                    .FirstOrDefaultAsync();
            }
            else if (paramType == enTeachingAssignmentType.GroupID) // GroupID
            {
                return await _dbSet.Where(i => i.GroupID == clsUtil.ToGuid(param))
                   .FirstOrDefaultAsync();
            }
            else
            {
                return await _dbSet.Where(i => i.SubjectID == clsUtil.ToGuid(param))
                                    .FirstOrDefaultAsync();
            }
        }

        public async Task<List<TeachingAssignmentInfo>> GetList()
        {
            return await _dbSet.Where(i => i.DeletedAt == null)
               .Select(i => new TeachingAssignmentInfo
               {
                   EnrollmentID = i.Id.ToString(),
                   TeacherID = i.TeacherID.ToString(),
                   TeacherName = i.Teacher.Person.FirstName + " " + i.Teacher.Person.FatherName + " " + i.Teacher.Person.LastName,
                   GroupID = i.GroupID.ToString(),
                   GroupName = i.Group.GroupName,
                   SubjectID = i.SubjectID.ToString(),
                   SubjectName = i.Subject.Name,
                   EnrollmentDate = i.Date
               }).ToListAsync();
        }

        public async Task<Paging<TeachingAssignmentInfo>> GetListFilter(GetAllTeachingAssignmentQuery requset)
        {
            return await _dbSet.Where(i => i.DeletedAt == null)
                   .Select(i => new TeachingAssignmentInfo
                   {
                       EnrollmentID = i.Id.ToString(),
                       TeacherID = i.TeacherID.ToString(),
                       TeacherName = i.Teacher.Person.FirstName + " " + i.Teacher.Person.FatherName + " " + i.Teacher.Person.LastName,
                       GroupID = i.GroupID.ToString(),
                       GroupName = i.Group.GroupName,
                       SubjectID = i.SubjectID.ToString(),
                       SubjectName = i.Subject.Name,
                       EnrollmentDate = i.Date
                   }).GridifyAsync(requset);
        }

        public async Task<Paging<TeacherGroupInfo>> GetTeacherGroups(GetTeacherGroupsQuery requset,string TeacherID)
        {
            return await _dbSet.Where(i => i.DeletedAt == null&& i.TeacherID == clsUtil.ToGuid( TeacherID))
               .Select(i => new TeacherGroupInfo
               {
                   EnrollmentID = i.Id.ToString(),
                   TeacherID = i.TeacherID.ToString(),
                   TeacherName = i.Teacher.Person.FirstName + " " + i.Teacher.Person.FatherName + " " + i.Teacher.Person.LastName,
                   GroupID = i.GroupID.ToString(),
                   GroupName = i.Group.GroupName,
                   SubjectID = i.SubjectID.ToString(),
                   SubjectName = i.Subject.Name,
                   EnrollmentDate = i.Date,
                   CalssName = i.Group.InstituteClass.Class.Name,
                   ClassLevel = i.Group.InstituteClass.Class.Level,

               }).GridifyAsync(requset);
        }

        public async Task<bool> IsExistBy(string param, enTeachingAssignmentType paramType)
        {
            if (paramType == enTeachingAssignmentType.ID)
            {
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
                  
            }
            else if (paramType == enTeachingAssignmentType.TeacherID)
            {
                return await _dbSet.AnyAsync(i => i.TeacherID == clsUtil.ToGuid(param))
                    ;
            }
            else if (paramType == enTeachingAssignmentType.GroupID) // GroupID
            {
                return await _dbSet.AnyAsync(i => i.GroupID == clsUtil.ToGuid(param))
                  ;
            }
            else
            {
                return await _dbSet.AnyAsync(i => i.SubjectID == clsUtil.ToGuid(param))
                                    ;
            }
        }

        public async Task<bool> IsTeacherEnrolled(string teachetID, string groupID, string subjectID)
        {
            return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(teachetID) && i.GroupID == clsUtil.ToGuid(groupID)
            && i.SubjectID == clsUtil.ToGuid(subjectID));
        }

        public async Task<bool> Update(clsTeachingAssignment entity)
        {
            _dbSet.Update(entity);
            return true;
        }
    }
}
