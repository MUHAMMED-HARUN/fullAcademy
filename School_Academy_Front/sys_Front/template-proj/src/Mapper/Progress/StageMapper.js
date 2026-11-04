export default class StageMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      StageID: dataSchema.stageID,
      StageName: dataSchema.name,
      StageNote: dataSchema.note,
      ClassID: dataSchema.classID,
      PrevStageID: dataSchema.prevStageID || null,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      stageID: DataReq.StageID,
      name: DataReq.StageName,
      note: DataReq.StageNote,
      classID: DataReq.ClassID,
      className: DataReq.ClassName,
      prevStageID: DataReq.PrevStageID || "",
      prevStageName: DataReq.PrevStageName || "",
    };
  }

  static mapListApiToStages(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        stageID: list.StageID,
        name: list.StageName,
        note: list.StageNote,
        classID: list.ClassID,
        className: list.ClassName,
        prevStageID: list.PrevStageID || "",
        prevStageName: list.PrevStageName || "",
      };
    });
  }

  static mapListApiToStageLookups(dataList) {
    return this.mapListApiToStages(dataList);
  }
}
