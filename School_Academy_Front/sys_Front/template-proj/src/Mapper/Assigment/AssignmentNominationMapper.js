export default class AssignmentNominationMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      AssignmentNominationID: dataSchema.assignmentNominationID,
      StudentEnrollmentID: dataSchema.studentEnrollmentID,
      AssignmentScopeID: dataSchema.assignmentScopeID,
      Date: dataSchema.date,
      NominatedByID: dataSchema.nominatedByID,
      NominatedByType: dataSchema.nominatedByType,
      NominationState: dataSchema.nominationState,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      assignmentNominationID: DataReq.AssignmentNominationID,
      studentEnrollmentID: DataReq.StudentEnrollmentID,
      studentName: DataReq.StudentName,
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
        studentEnrollmentID: list.StudentEnrollmentID,
        studentName: list.StudentName,
        assignmentScopeID: list.AssignmentScopeID,
        assignmentScopeName: list.AssignmentScopeName,
        nominationState: list.NominationState,
        date: list.Date,
        canStartAssigment: list.CanStartAssigment,
      };
    });
  }
}
