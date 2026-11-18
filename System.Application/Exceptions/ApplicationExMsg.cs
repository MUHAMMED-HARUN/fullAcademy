
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
        public static string PersonNationalNumberExists { get; } = "A person with this national number already exists.";
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
        public static string FeeStructureExistByName { get; internal set; } = "A fee structure with this name already exists.";
        public static string FeeStructureUsedCannotUpdate { get; internal set; } = "A fee structure already used by invoices cannot be changed or deleted. Create a new fee structure instead.";
        public static string InvoiceCancelled { get; internal set; } = "Cannot add a payment to a cancelled invoice.";
        public static string PaymentExceedsRemaining { get; internal set; } = "Payment amount cannot exceed the remaining amount.";
        public static string AcademicPlanProviderInstituteNotFound { get; internal set; } = "Provider institute was not found.";
        public static string AcademicPlanScopeNotFound { get; internal set; } = "Academic plan scope was not found.";
        public static string AcademicPlanScopeHasNoStageInfo { get; internal set; } = "Academic plan scope has no stage info items.";
        public static string AcademicPlanProviderMismatch { get; internal set; } = "Provider institute does not match the selected class.";
        public static string AcademicPlanNotEditable { get; internal set; } = "Archived academic plans cannot be edited.";
        public static string AcademicPlanActivateOnlyDraft { get; internal set; } = "Only draft academic plans can be activated.";
        public static string AcademicPlanDeleteOnlyDraft { get; internal set; } = "Only draft academic plans can be deleted.";
        public static string StudentAcademicEnrollmentStudentNotFound { get; internal set; } = "Student was not found.";
        public static string StudentAcademicEnrollmentPlanUnavailable { get; internal set; } = "The academic plan is not active or is unavailable for enrollment.";
        public static string StudentAcademicEnrollmentOutsidePlanValidity { get; internal set; } = "The enrollment validity period must be within the academic plan validity period.";
        public static string StudentAcademicEnrollmentAlreadyExists { get; internal set; } = "The student already has an active enrollment in this academic plan.";
        public static string StudentAcademicEnrollmentNotFound { get; internal set; } = "Student academic enrollment was not found.";
        public static string StudentAcademicEnrollmentGroupNotEligible { get; internal set; } = "The selected group is outside the academic plan scope or the enrollment is not valid on the selected date.";
        public static string StudentStage_PreviousStageNotIncluded { get; internal set; } = "The previous stage is not included in the student's academic enrollment snapshot.";
        public static string StudentStage_PreviousStageNotCompleted { get; internal set; } = "The previous stage must be completed before starting this stage.";
        public static string StudentAcademicEnrollmentNotActive { get; internal set; } = "The student academic enrollment is not active on the selected date.";
        public static string StudentAcademicEnrollmentStageNotIncluded { get; internal set; } = "The stage is not included in this student's academic enrollment snapshot.";
        public static string StudentAcademicEnrollmentStageInfoNotIncluded { get; internal set; } = "The stage info is not included in this student's academic enrollment snapshot.";
        public static string AssignmentNominationScopeNotEligible { get; internal set; } = "The assignment scope is not eligible for this academic enrollment.";
        public static string ResultScoreOutOfRange { get; internal set; } = "The result score is outside the allowed range for this scope info.";
        public static string GroupInstituteClassNotFound { get; internal set; } = "The selected institute class was not found.";
        public static string GroupHasActiveRelationships { get; internal set; } = "The group has active student memberships or teaching assignments and cannot be moved or deleted.";
    }
}
