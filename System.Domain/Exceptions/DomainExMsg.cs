

namespace System.Domain.Exceptions
{
    public static class DomainExMsg
    {
        public static DomainExceptionObj Address_CountryID_Required = new DomainExceptionObj("Address_CountryID_Required", "Country ID is required.");

        public static DomainExceptionObj Address_CityID_Required = new DomainExceptionObj("Address_CountryID_Required", "Country ID is required."); 
        public static DomainExceptionObj Address_DistrictID_Required = new DomainExceptionObj("Address_CountryID_Required", "Country ID is required.");
        public static DomainExceptionObj Address_NeighborhoodID_Required = new DomainExceptionObj("Address_CountryID_Required", "Country ID is required.");

        public static DomainExceptionObj Country_Name_Required = new DomainExceptionObj("Country_Name_Required", "Country Name is Required");
        public static DomainExceptionObj Country_Code_Required = new DomainExceptionObj("Country_Code_Required", "Country Code is Required");

        public static DomainExceptionObj District_CityID_Required = new DomainExceptionObj("District_CityID_Required", "District City ID is Required");
        public static DomainExceptionObj District_Name_Required { get; internal set; }  
        public static DomainExceptionObj City_CountryID_Required { get; internal set; }
        public static DomainExceptionObj City_Name_Required { get; internal set; }
        public static DomainExceptionObj Neighborhood_DistrictID_Required { get; internal set; }
        public static DomainExceptionObj Neighborhood_Name_Required { get; internal set; }
        public static DomainExceptionObj Neighborhood_District_IDRequired { get; internal set; }
        public static DomainExceptionObj Person_FirstName { get; internal set; }
        public static DomainExceptionObj Person_FatherName { get; internal set; }
        public static DomainExceptionObj Person_LastName { get; internal set; }
        public static DomainExceptionObj Person_MotherName { get; internal set; }
        public static DomainExceptionObj Person_MotherLastName { get; internal set; }
        public static DomainExceptionObj Person_NationalNumber { get; internal set; }
        public static DomainExceptionObj Person_BirthDate { get; internal set; }
        public static DomainExceptionObj Student_PersonIDRequired { get; internal set; }
        public static DomainExceptionObj Teacher_PersonIDRequired { get; internal set; }
        public static DomainExceptionObj Institute_NameRequired { get; internal set; }
        public static DomainExceptionObj Institute_AddressIDRequired { get; internal set; }
        public static DomainExceptionObj Program_NameRequired { get; internal set; }
        public static DomainExceptionObj Class_NameRequired { get; internal set; }
        public static DomainExceptionObj Class_LevelRequired { get; internal set; }
        public static DomainExceptionObj Class_ProgramIDRequired { get; internal set; }
        public static DomainExceptionObj Class_NoteRequired { get; internal set; }
        public static DomainExceptionObj InstituteClass_InstituteIDRequired { get; internal set; }
        public static DomainExceptionObj InstituteClass_ClassIDRequired { get; internal set; }
        public static DomainExceptionObj Group_InstituteClassIDRequired { get; internal set; }
        public static DomainExceptionObj Group_NameRequired { get; internal set; }
        public static DomainExceptionObj Group_CodeRequired { get; internal set; }
        public static DomainExceptionObj StudentEnrollment_StudentIDRequired { get; internal set; }
        public static DomainExceptionObj StudentEnrollment_GroupIDRequired { get; internal set; }
        public static DomainExceptionObj StudentEnrollment_DateRequired { get; internal set; }
        public static DomainExceptionObj Subject_NameRequired { get; internal set; }
        public static DomainExceptionObj TeachingAssignment_TeacherIDRequired { get; internal set; }
        public static DomainExceptionObj TeachingAssignment_GroupIDRequired { get; internal set; }
        public static DomainExceptionObj TeachingAssignment_SubjectIDRequired { get; internal set; }
        public static DomainExceptionObj Stage_NameRequired { get; internal set; }
        public static DomainExceptionObj Stage_ProgramIDRequired { get; internal set; }
        public static DomainExceptionObj Stage_ClassIDRequired { get; internal set; }
        public static DomainExceptionObj Stage_PrevStageDRequired { get; internal set; }
        public static DomainExceptionObj Stage_PrevClassIDRequired { get; internal set; }
        public static Exception StudentStage_StudentIDRequired { get; internal set; }
        public static DomainExceptionObj StageInfo_FromMustBePositive { get; internal set; }
        public static DomainExceptionObj StageInfo_ToMustBePositive { get; internal set; }
        public static DomainExceptionObj StageInfo_ToMustBeGreaterThanFrom { get; internal set; }
        public static DomainExceptionObj StageInfo_StageIDRequired { get; internal set; }
        public static DomainExceptionObj StageInfo_MatterIDRequired { get; internal set; }
        public static DomainExceptionObj DailyEvaluation_StudentIDRequired { get; internal set; }
        public static DomainExceptionObj DailyEvaluation_DateRequired { get; internal set; }
        public static DomainExceptionObj DailyEvaluation_FromInvalid { get; internal set; }
        public static DomainExceptionObj DailyEvaluation_ToInvalid { get; internal set; }
        public static DomainExceptionObj DailyEvaluation_UnitTypeIDRequired { get; internal set; }
        public static DomainExceptionObj DailyEvaluation_StageInfoIDRequired { get; internal set; }
        public static DomainExceptionObj Assignment_AssignmentNominationIDRequired { get; internal set; }
        public static DomainExceptionObj Assignment_CommitteeIDRequired { get; internal set; }
        public static DomainExceptionObj Result_AssignmentScopeInfoIDRequired { get; internal set; }
        public static DomainExceptionObj Result_ScoreMustBeNonNegative { get; internal set; }
        public static DomainExceptionObj Result_AssignmentIDRequired { get; internal set; }
        public static DomainExceptionObj AssignmentScope_NameRequired { get; internal set; }
        public static DomainExceptionObj AssignmentScope_StageIDRequired { get; internal set; }
        public static DomainExceptionObj AssignmentScopeInfo_AssignmentScopeIDRequired { get; internal set; }
        public static DomainExceptionObj AssignmentScopeInfo_StageInfoIDRequired { get; internal set; }
        public static DomainExceptionObj AssignmentScopeInfo_WeightInvalid { get; internal set; }
        public static DomainExceptionObj AssignmentScopeInfo_MinPassScoreInvalid { get; internal set; }
        public static DomainExceptionObj AssignmentScopeInfo_MaxScoreInvalid { get; internal set; }
        public static DomainExceptionObj AssignmentNomination_StudentEnrollmentIDRequired { get; internal set; }
        public static DomainExceptionObj AssignmentNomination_AssignmentScopeIDRequired { get; internal set; }
        public static DomainExceptionObj AssignmentNomination_NominatedByIDRequired { get; internal set; }
        public static DomainExceptionObj Attendance_DateRequired { get; internal set; }
        public static DomainExceptionObj Attendance_CheckOutBeforeCheckIn { get; internal set; }
        public static DomainExceptionObj Attendance_AttendeeIDRequired { get; internal set; }
        public static DomainExceptionObj LeaveRequest_ApproverRequired { get; internal set; }
        public static DomainExceptionObj LeaveRequest_CannotCancelApprovedLeave { get; internal set; }
        public static DomainExceptionObj LeaveRequest_RejectionReasonRequired { get; internal set; }
        public static DomainExceptionObj LeaveRequest_ReasonRequired { get; internal set; }
        public static DomainExceptionObj LeaveRequest_EndDateInvalid { get; internal set; }
        public static DomainExceptionObj LeaveRequest_StartDateRequired { get; internal set; }
        public static DomainExceptionObj LeaveRequest_ApplicantIDRequired { get; internal set; }
        public static DomainExceptionObj LeaveRequest_EntityIDRequired { get; internal set; }
        public static DomainExceptionObj LeaveRequest_StartDateAfterEndDate { get; internal set; }
        public static DomainExceptionObj Attendance_LeaveRequestIDRequired { get; internal set; }
        public static DomainExceptionObj Guardian_PersonIDRequired { get; internal set; }
        public static DomainExceptionObj StudentGuardian_StudentIDRequired { get; internal set; }
        public static DomainExceptionObj StudentGuardian_GuardianIDRequired { get; internal set; }

        // Finance
        public static DomainExceptionObj FeeStructure_NameRequired = new DomainExceptionObj("FeeStructure_NameRequired", "Fee structure name is required.");
        public static DomainExceptionObj FeeStructure_AmountInvalid = new DomainExceptionObj("FeeStructure_AmountInvalid", "Fee structure amount must be greater than or equal to 0.");

        public static DomainExceptionObj StudentInvoice_StudentIDRequired = new DomainExceptionObj("StudentInvoice_StudentIDRequired", "Student ID is required.");
        public static DomainExceptionObj StudentInvoice_FeeStructureIDRequired = new DomainExceptionObj("StudentInvoice_FeeStructureIDRequired", "Fee structure ID is required.");
        public static DomainExceptionObj StudentInvoice_DueDateRequired = new DomainExceptionObj("StudentInvoice_DueDateRequired", "Due date is required.");
        public static DomainExceptionObj StudentInvoice_DueDateBeforeIssueDate = new DomainExceptionObj("StudentInvoice_DueDateBeforeIssueDate", "Due date cannot be before issue date.");
        public static DomainExceptionObj StudentInvoice_OriginalAmountInvalid = new DomainExceptionObj("StudentInvoice_OriginalAmountInvalid", "Original amount must be greater than or equal to 0.");
        public static DomainExceptionObj StudentInvoice_DiscountAmountInvalid = new DomainExceptionObj("StudentInvoice_DiscountAmountInvalid", "Discount amount must be greater than or equal to 0.");
        public static DomainExceptionObj StudentInvoice_DiscountExceedsOriginal = new DomainExceptionObj("StudentInvoice_DiscountExceedsOriginal", "Discount cannot exceed the original amount.");
        public static DomainExceptionObj StudentInvoice_DiscountExceedsUnpaid = new DomainExceptionObj("StudentInvoice_DiscountExceedsUnpaid", "Discount cannot exceed the unpaid amount.");
        public static DomainExceptionObj StudentInvoice_Cancelled = new DomainExceptionObj("StudentInvoice_Cancelled", "Cannot modify a cancelled invoice.");

        public static DomainExceptionObj Payment_StudentInvoiceIDRequired = new DomainExceptionObj("Payment_StudentInvoiceIDRequired", "Student invoice ID is required.");
        public static DomainExceptionObj Payment_AmountMustBePositive = new DomainExceptionObj("Payment_AmountMustBePositive", "Payment amount must be greater than 0.");
        public static DomainExceptionObj Payment_AmountExceedsRemaining = new DomainExceptionObj("Payment_AmountExceedsRemaining", "Payment amount cannot exceed the remaining amount.");
    }
}
