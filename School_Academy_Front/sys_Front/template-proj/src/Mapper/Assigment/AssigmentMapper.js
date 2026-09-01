export default class StageMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      AssignmentID: dataSchema.assignmentID,
      AssignmentNominationID: dataSchema.assignmentNominationID,
      CommitteeID: dataSchema.committeeID,
      CommitteeType: dataSchema.committeeType,
      ScheduledDate: dataSchema.scheduledDate,
      AssignmentState: dataSchema.assignmentState,
      IsPassed: dataSchema.isPassed,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      assignmentID: DataReq.AssignmentID,
      assignmentNominationID: DataReq.AssignmentNominationID,
      scheduledDate: DataReq.ScheduledDate,
      assignmentState: DataReq.AssignmentState,
      studentName: DataReq.StudentName,
      assigmentScopeName: DataReq.AssigmentScopeName,
      isPassed: DataReq.IsPassed,
      assignmentScopeID: DataReq.AssignmentScopeID,
    };
  }

  static mapListApiToAssignments(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        assignmentID: list.AssignmentID,
        assignmentNominationID: list.AssignmentNominationID,
        scheduledDate: list.ScheduledDate,
        assignmentState: list.AssignmentState,
        studentName: list.StudentName,
        assigmentScopeName: list.AssigmentScopeName,
        isPassed: list.IsPassed,
        assignmentScopeID: list.AssignmentScopeID,
      };
    });
  }
}
