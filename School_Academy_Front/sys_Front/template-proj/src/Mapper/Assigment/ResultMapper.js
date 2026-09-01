export default class ResultMapper {
  static mapUpsertToApi(dataSchema) {
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
      assignmentScopeInfoID: DataReq.AssignmentScopeInfoID,
      assignmentScopeName: DataReq.AssignmentScopeName,
      stageInfoMatter: DataReq.stageInfoMatter,
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
        assignmentScopeInfoID: list.AssignmentScopeInfoID,
        assignmentScopeName: list.AssignmentScopeName,
        stageInfoMatter: list.stageInfoMatter,
        score: list.Score,
        notes: list.Notes,
        studentName: list.StudentName,
      };
    });
  }
}
