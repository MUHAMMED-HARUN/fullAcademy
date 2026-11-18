export default class ResultMapper {
  static mapCreateToApi(dataSchema) {
    return {
      AssignmentID: dataSchema.assignmentID,
      AssignmentScopeInfoID: dataSchema.assignmentScopeInfoID,
      Score: dataSchema.score,
      Notes: dataSchema.notes,
    };
  }

  static mapUpdateToApi(dataSchema) {
    return {
      ResultID: dataSchema.resultID,
      AssignmentID: dataSchema.assignmentID,
      AssignmentScopeInfoID: dataSchema.assignmentScopeInfoID,
      Score: dataSchema.score,
      Notes: dataSchema.notes,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      resultID: DataReq.ResultID,
      assignmentID: DataReq.AssignmentID,
      studentAcademicEnrollmentID: DataReq.StudentAcademicEnrollmentID,
      academicPlanName: DataReq.AcademicPlanName,
      assignmentScopeInfoID: DataReq.AssignmentScopeInfoID,
      assignmentScopeName: DataReq.AssignmentScopeName,
      stageInfoMatter: DataReq.StageInfoMatter,
      maxScore: DataReq.MaxScore,
      minPassScore: DataReq.MinPassScore,
      score: DataReq.Score,
      notes: DataReq.Notes,
      studentName: DataReq.StudentName,
    };
  }

  static mapListApiToResult(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        resultID: list.ResultID,
        assignmentID: list.AssignmentID,
        studentAcademicEnrollmentID: list.StudentAcademicEnrollmentID,
        academicPlanName: list.AcademicPlanName,
        assignmentScopeInfoID: list.AssignmentScopeInfoID,
        assignmentScopeName: list.AssignmentScopeName,
        stageInfoMatter: list.StageInfoMatter,
        maxScore: list.MaxScore,
        minPassScore: list.MinPassScore,
        score: list.Score,
        notes: list.Notes,
        studentName: list.StudentName,
      };
    });
  }
}
