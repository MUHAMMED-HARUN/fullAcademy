export default class AssignmentNominationMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      AssignmentNominationID: dataSchema.assignmentNominationID,
      StudentAcademicEnrollmentID: dataSchema.studentAcademicEnrollmentID,
      AssignmentScopeID: dataSchema.assignmentScopeID,
      Date: dataSchema.date,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      assignmentNominationID: DataReq.AssignmentNominationID,
      studentAcademicEnrollmentID: DataReq.StudentAcademicEnrollmentID,
      studentName: DataReq.StudentName,
      academicPlanName: DataReq.AcademicPlanName,
      assignmentScopeID: DataReq.AssignmentScopeID,
      assignmentScopeName: DataReq.AssignmentScopeName,
      nominationState: DataReq.NominationState,
      date: DataReq.Date,
    };
  }

  static mapListApiToAssignmentNomination(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        assignmentNominationID: list.AssignmentNominationID,
        studentAcademicEnrollmentID: list.StudentAcademicEnrollmentID,
        studentName: list.StudentName,
        academicPlanName: list.AcademicPlanName,
        assignmentScopeID: list.AssignmentScopeID,
        assignmentScopeName: list.AssignmentScopeName,
        nominationState: list.NominationState,
        date: list.Date,
        canStartAssigment: list.CanStartAssigment,
      };
    });
  }
}
