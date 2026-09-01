
using System;
using System.Application.Common;
using System.Domain.Exceptions;

namespace System.Application.Exceptions
{
    public static class ApplicationExMsg
    {
        internal static readonly string failToFindProgram;
        public static ApplicationExceptionObj SystemError = new ApplicationExceptionObj("SYSTEM_ERROR", "An unexpected error occurred.");
        internal static string failUpdateInstitute;
        internal static string failToFindInstitue;
        internal static ApplicationExceptionObj failToFindClass = new ApplicationExceptionObj("SYSTEM_ERROR", "An unexpected error occurred.");
        internal static string failUpdateClass;
        internal static string failToFindInstituteClass;
        internal static string failToFindSession;
        internal static string failToFindStage;
        internal static string failToFindStudentStage;
        internal static string failToFindStageInfo;

        public static string? RegistritionFaild { get; internal set; }
        public static string? InvalidLogin { get; internal set; }
        public static string CountryIsExist { get; internal set; }
        public static string FailAdd { get; internal set; }
        public static string CountryNotFound { get; internal set; }
        public static string CityNotFound { get; internal set; }
        public static string CityIsExist { get; internal set; }
        public static string DistrictIsExist { get; internal set; }
        public static string DistrictNotFound { get; internal set; }
        public static string NeighborhoodIsExist { get; internal set; }
        public static string FailAddNeighborhood { get; internal set; }
        public static string NeighborhoodNotFound { get; internal set; }
        public static string FailAddAddress { get; internal set; }
        public static string AddressNotFound { get; internal set; }
        public static string PersonIsAlreadyStudent { get; internal set; }
        public static string FailAddPerson { get; internal set; }
        public static string FailToFindStudent { get; internal set; }
        public static string InstituteExistByName { get; internal set; }
        public static string ProgramExistByName { get; internal set; }
        public static string ClassExistByName { get; internal set; }
        public static string ClassIsAlreadyInInstitute { get; set; }
        public static string GroupExistByName { get; set; }
        public static string GroupExistByCode { get; set; }
        public static string GroupNotFound { get; set; }
        public static string StudentEnrollmentNotFound { get; set; }
        public static string StudentAlreadyEnrolledInGroup { get; set; }
        public static string FailToFindTeacher { get; internal set; }
        public static string PersonIsAlreadyTeacher { get; internal set; }
        public static string SessionNameExistInType { get; internal set; }
        public static string SessionExistByName { get; internal set; }
        public static string TeachingAssignmentNotFound { get; internal set; }
        public static string TeacherAlreadyEnrolledInGroup { get; internal set; }
        public static string StageNameExistInProgram { get; internal set; }
        public static string StageExistByName { get; internal set; }
        public static TResult<string> StudentIsAlreadyInStage { get; internal set; }
        public static string StudentStageNotFound { get; internal set; }
        public static string failToFindDailyEvaluation { get; internal set; }
        public static string DailyEvaluationRangeIsOutOFRange { get; internal set; }
        public static string SessionParentIsMatter { get; internal set; }
        public static string AccessDinede { get; internal set; }
        public static string NotFound { get; internal set; } = "Record not found";
        public static string AssignmentNotFound { get; internal set; }
    }
}
