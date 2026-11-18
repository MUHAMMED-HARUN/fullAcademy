export default class StageMapper {
  static mapCreateToApi(dataSchema) {
    return {
      AssignmentNominationID: dataSchema.assignmentNominationID,
      ScheduledDate: dataSchema.scheduledDate,
    };
  }

  static mapUpdateToApi(dataSchema) {
    return {
      AssignmentID: dataSchema.assignmentID,
      ScheduledDate: dataSchema.scheduledDate,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      assignmentID: DataReq.AssignmentID,
      assignmentNominationID: DataReq.AssignmentNominationID,
      studentAcademicEnrollmentID: DataReq.StudentAcademicEnrollmentID,
      academicPlanName: DataReq.AcademicPlanName,
      scheduledDate: DataReq.ScheduledDate,
      assignmentState: DataReq.AssignmentState,
      committeeID: DataReq.CommitteeID,
      committeeType: DataReq.CommitteeType,
      studentName: DataReq.StudentName,
      assigmentScopeName: DataReq.AssigmentScopeName,
      isPassed: DataReq.IsPassed,
      assignmentScopeID: DataReq.AssignmentScopeID,
      totalScore: DataReq.TotalScore,
    };
  }

  static mapListApiToAssignments(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        assignmentID: list.AssignmentID,
        assignmentNominationID: list.AssignmentNominationID,
        studentAcademicEnrollmentID: list.StudentAcademicEnrollmentID,
        academicPlanName: list.AcademicPlanName,
        scheduledDate: list.ScheduledDate,
        assignmentState: list.AssignmentState,
        committeeID: list.CommitteeID,
        committeeType: list.CommitteeType,
        studentName: list.StudentName,
        assigmentScopeName: list.AssigmentScopeName,
        isPassed: list.IsPassed,
        assignmentScopeID: list.AssignmentScopeID,
        canAddResult: list.CanAddResult,
        totalScore: list.TotalScore,
      };
    });
  }
}
