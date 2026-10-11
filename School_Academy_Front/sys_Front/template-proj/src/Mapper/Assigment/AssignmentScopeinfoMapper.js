export default class AssignmentScopeInfoMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      AssignmentScopeInfoID: dataSchema.assignmentScopeInfoID,
      MaxScore: dataSchema.maxScore,
      MinPassScore: dataSchema.minPassScore,
      Weight: dataSchema.weight,
      StageInfoID: dataSchema.stageInfoID,
      AssignmentScopeID: dataSchema.assignmentScopeID,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      assignmentScopeInfoID: DataReq.AssignmentScopeInfoID,
      maxScore: DataReq.MaxScore,
      minPassScore: DataReq.MinPassScore,
      weight: DataReq.Weight,
      stageInfoID: DataReq.StageInfoID,
      stageInfoMatterNameName: DataReq.StageInfoMatterNameName,
      assignmentScopeID: DataReq.AssignmentScopeID,
      assignmentScopeName: DataReq.AssignmentScopeName,
    };
  }
  static MapFromApiDTOToAssigmetSelect(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        assignmentScopeInfoID: list.AssignmentScopeInfoID,
        maxScore: list.MaxScore,
        minPassScore: list.MinPassScore,
        stageInfoMatterNameName: list.StageInfoMatterNameName,
      };
    });
  }
  static mapListApiToAssignmentScopeInfos(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        assignmentScopeInfoID: list.AssignmentScopeInfoID,
        maxScore: list.MaxScore,
        minPassScore: list.MinPassScore,
        weight: list.Weight,
        stageInfoID: list.StageInfoID,
        stageInfoMatterNameName: list.StageInfoMatterNameName,
        assignmentScopeID: list.AssignmentScopeID,
        assignmentScopeName: list.AssignmentScopeName,
      };
    });
  }
}
