export default class AssignmentScopeMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      AssignmentScopeID: dataSchema.assignmentScopeID,
      Name: dataSchema.name,
      Notes: dataSchema.notes,
      StageID: dataSchema.stageID,
      PrevAssignmentScopeID: dataSchema.prevAssignmentScopeID,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      assignmentScopeID: DataReq.AssignmentScopeID,
      name: DataReq.Name,
      notes: DataReq.Notes,
      stageID: DataReq.StageID,
      stageName: DataReq.StageName,
      prevAssignmentScopeID: DataReq.PrevAssignmentScopeID,
      prevAssignmentScopeName: DataReq.PrevAssignmentScopeName,
    };
  }

  static mapListApiToAssignmentScopes(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        assignmentScopeID: list.AssignmentScopeID,
        name: list.Name,
        notes: list.Notes,
        stageID: list.StageID,
        stageName: list.StageName,
        prevAssignmentScopeID: list.PrevAssignmentScopeID,
        prevAssignmentScopeName: list.PrevAssignmentScopeName,
      };
    });
  }

  static mapListApiToCanNomenate(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        assignmentScopeID: list.AssignmentScopeID,
        name: list.Name,
      };
    });
  }
}
