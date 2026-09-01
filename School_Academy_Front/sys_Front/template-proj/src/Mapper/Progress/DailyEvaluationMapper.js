export default class DailyEvaluationMapperMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      DailyEvaluationID: dataSchema.dailyEvaluationID,
      To: dataSchema.to,
      StageInfoID: dataSchema.stageInfoID,
      StudentEnrollmentID: dataSchema.studentEnrollmentID,
      Date: dataSchema.date,
      LevelType: dataSchema.levelType,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      dailyEvaluationID: DataReq.DailyEvaluationID,
      studentEnrollmentID: DataReq.StudentEnrollmentID,
      studentName: DataReq.StudentName,
      groupName: DataReq.GroupName,
      groupID: DataReq.GroupID,
      from: DataReq.From,
      to: DataReq.To,
      date: DataReq.Date,
      stageInfoID: DataReq.StageInfoID,
      levelType: DataReq.LevelType,
    };
  }

  static mapListApiToDailyEvaluation(dataList) {
    if (!dataList) return [];

    // التحقق مما إذا كانت القائمة مصفوفة مباشرة أم كائن يحتوي على items القادم من Gridify
    const items = Array.isArray(dataList)
      ? dataList
      : dataList.items || dataList.data || [];

    return items.map((list) => {
      return {
        dailyEvaluationID: list.DailyEvaluationID,
        studentEnrollmentID: list.StudentEnrollmentID,
        studentName: list.StudentName,
        groupName: list.GroupName,
        groupID: list.GroupID,
        from: list.From,
        to: list.To,
        date: list.Date,
        stageInfoID: list.StageInfoID,
        levelType: list.LevelType,
      };
    });
  }
}
