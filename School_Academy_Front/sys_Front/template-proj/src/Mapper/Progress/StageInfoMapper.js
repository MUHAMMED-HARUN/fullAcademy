export default class StageInfoMapperMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      StageInfoID: dataSchema.stageInfoID,
      From: dataSchema.from,
      To: dataSchema.to,
      StageID: dataSchema.stageID,
      MatterID: dataSchema.matterID,
      UniteType: dataSchema.uniteType,
      ProgressType: dataSchema.progressType,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      stageInfoID: DataReq.StageInfoID,
      from: DataReq.From,
      to: DataReq.To,
      stageID: DataReq.StageID,
      matterID: DataReq.MatterID,
      matterName: DataReq.MatterName,
      stageName: DataReq.StageName,
      uniteType: DataReq.UniteType,
      uniteTypeName: DataReq.UniteTypeName,
      progressType: DataReq.ProgressType,
      progressTypeName: DataReq.ProgressTypeName,
    };
  }

  static mapListApiToStagesInfo(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        stageInfoID: list.StageInfoID,
        from: list.From,
        to: list.To,
        stageID: list.StageID,
        matterID: list.MatterID,
        matterName: list.MatterName,
        stageName: list.StageName,
        uniteType: list.UniteType,
        uniteTypeName: list.UniteTypeName,
        progressType: list.ProgressType,
        progressTypeName: list.ProgressTypeName,
      };
    });
  }
}
