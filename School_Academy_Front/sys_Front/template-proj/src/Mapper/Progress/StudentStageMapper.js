export default class StudentStageMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      StudentStageID: dataSchema.studentStageID,
      StageID: dataSchema.stageID,
      EnrollmentStudentID: dataSchema.enrollmentStudentID,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      studentStageID: DataReq.StudentStageID,
      stageID: DataReq.StageID,
      stageName: DataReq.StageName,
      enrollmentStudentID: DataReq.EnrollmentStudentID,
      studentName: DataReq.StudentName,
      groupName: DataReq.GroupName,
      studentStageState: DataReq.StudentStageState,
      studentStageDate: DataReq.StudentStageDate,
    };
  }

  static mapListApiToStudentStage(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        studentStageID: list.StudentStageID,
        stageID: list.StageID,
        stageName: list.StageName,
        enrollmentStudentID: list.EnrollmentStudentID,
        studentName: list.StudentName,
        groupName: list.GroupName,
        studentStageState: list.StudentStageState,
        studentStageDate: list.StudentStageDate,
      };
    });
  }
}
