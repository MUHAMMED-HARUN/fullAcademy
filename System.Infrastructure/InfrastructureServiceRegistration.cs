

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Person;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Abstractions.Enrollment;
using System.Application.Abstractions.Assignment;
using System.Infrastructure.Identity;
using System.Infrastructure.Persistence;
using System.Infrastructure.Repositories.Address;
using System.Infrastructure.Repositories.Auth;
using System.Infrastructure.Repositories.Institute.Institute;
using System.Infrastructure.Repositories.Person;
using System.Infrastructure.Repositories.Enrollment;
using System.Infrastructure.Repositories.Assignment;
using System.Reflection;
using System.Infrastructure.Repositories.Progress;
using System.Application.Abstractions.Progress;

namespace System.Infrastructure
{
        public static class InfrastructureServiceRegistration
        {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {

                services.AddDataProtection();
                services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(
                            configuration.GetConnectionString("cs")));
                // 2. تسجيل الـ Identity مخصصاً للـ API (AddIdentityCore)
                services.AddIdentityCore<ApplicationUser>(options =>
                {
                    // شروط كلمة المرور والحساب (اختياري)
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<ApplicationRole>() // لإضافة دعم الأدوار
                .AddEntityFrameworkStores<AppDbContext>() // ربطه بقاعدة البيانات
.AddDefaultTokenProviders();

                services.AddScoped<IUnitOfWork, UnitOfWork.UOW>();
                services.AddScoped<IIdentityService, IdentityService>();
                services.AddScoped<IJwtTokenGenerator, JwtTokenGeneratorService>();

                // Address Session
                services.AddScoped<IAddressRepository, AddressService>();
                services.AddScoped<ICountryRepository, CountryService>();
                services.AddScoped<ICityRepository, CityService>();
                services.AddScoped<IDistrictRepository, DistrictService>();
                services.AddScoped<INeighborhoodRepository, NeighborhoodService>();

                // Person Serv
                services.AddScoped<IPersonRepository, PersonService>();
                services.AddScoped<IStudentRepository, StudentService>();
                services.AddScoped<ITeacherRepository, TeacherService>();


                // Institute Session
                services.AddScoped<IInstituteRepository, InstituteService>();
                services.AddScoped<IProgramRepository, ProgramService>();
                services.AddScoped<IClassRepository, ClassService>();
                services.AddScoped<IInstituteClassRepository, InstituteClassService>();
                services.AddScoped<IGroupRepository, GroupService>();
                services.AddScoped<IStudentEnrollmentRepository, StudentEnrollmentService>();
                services.AddScoped<ITeachingAssignmentRepository, TeachingAssignmentService>();
                // Progress Service
                services.AddScoped<ISessionRepository, SessionService>();
                services.AddScoped<IStageRepository, StageService>();
                services.AddScoped<IStageInfoRepository, StageInfoService>();
                services.AddScoped<IStudentStageRepository, StudentStageService>();
                services.AddScoped<IDailyEvaluationRepository, DailyEvaluationService>();

                // Assignment Service
                services.AddScoped<IAssignmentRepository, AssignmentService>();
                services.AddScoped<IAssignmentScopeRepository, AssignmentScopeService>();
                services.AddScoped<IAssignmentScopeInfoRepository, AssignmentScopeInfoService>();
                services.AddScoped<IAssignmentNominationRepository, AssignmentNominationService>();
                services.AddScoped<IResultRepository, ResultService>();

                return services;
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var e in ex.LoaderExceptions)
                {
                    Console.WriteLine("----");
                    Console.WriteLine(e.GetType().FullName);
                    Console.WriteLine(e.Message);
                }
                throw;
            }
        }
        }
}
