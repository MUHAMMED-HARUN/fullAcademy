

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
        public static DomainExceptionObj Stage_PrevStageDRequired { get; internal set; }
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
    }
}
