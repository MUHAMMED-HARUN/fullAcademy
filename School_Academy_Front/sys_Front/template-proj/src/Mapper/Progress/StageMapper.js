export default class StageMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      StageID: dataSchema.stageID,
      StageName: dataSchema.name,
      StageNote: dataSchema.note,
      ProgramID: dataSchema.programID,
      PrevStageID: dataSchema.prevStageID,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      stageID: DataReq.StageID,
      name: DataReq.StageName,
      note: DataReq.StageNote,
      programID: DataReq.ProgramID,
      programName: DataReq.ProgramName,
      prevStageID: DataReq.PrevStageID,
      prevStageName: DataReq.PrevStageName,
    };
  }

  static mapListApiToStages(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        stageID: list.StageID,
        name: list.StageName,
        note: list.StageNote,
        programID: list.ProgramID,
        programName: list.ProgramName,
        prevStageID: list.PrevStageID,
        prevStageName: list.PrevStageName,
      };
    });
  }
}
