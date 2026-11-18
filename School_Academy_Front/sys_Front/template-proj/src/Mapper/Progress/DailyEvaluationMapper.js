const levelTypeValues = Object.freeze({
  "سيء جدا": 0,
  "سيئ جدا": 0,
  سيء: 1,
  جيد: 2,
  جبد: 2,
  "جيد جدا": 3,
  ممتاز: 4,
});

export default class DailyEvaluationMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      DailyEvaluationID: dataSchema.dailyEvaluationID,
      To: dataSchema.to,
      StageInfoID: dataSchema.stageInfoID,
      StudentAcademicEnrollmentID: dataSchema.studentAcademicEnrollmentID,
      Date: dataSchema.date,
      LevelType: dataSchema.levelType,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      dailyEvaluationID: DataReq.DailyEvaluationID,
      studentAcademicEnrollmentID: DataReq.StudentAcademicEnrollmentID,
      studentName: DataReq.StudentName,
      academicPlanName: DataReq.AcademicPlanName,
      from: DataReq.From,
      to: DataReq.To,
      date: DataReq.Date,
      stageInfoID: DataReq.StageInfoID,
      stageInfoName: DataReq.StageInfoName,
      levelType: Number.isInteger(DataReq.LevelType)
        ? DataReq.LevelType
        : levelTypeValues[DataReq.LevelType] ?? 3,
      levelTypeName: DataReq.LevelTypeName || DataReq.LevelType,
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
        studentAcademicEnrollmentID: list.StudentAcademicEnrollmentID,
        studentName: list.StudentName,
        academicPlanName: list.AcademicPlanName,
        from: list.From,
        to: list.To,
        date: list.Date,
        stageInfoID: list.StageInfoID,
        stageInfoName: list.StageInfoName,
        levelType: Number.isInteger(list.LevelType)
          ? list.LevelType
          : levelTypeValues[list.LevelType] ?? 3,
        levelTypeName: list.LevelTypeName || list.LevelType,
      };
    });
  }
}
