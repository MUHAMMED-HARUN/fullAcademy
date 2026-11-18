export default class StudentStageMapper {
  static mapCreateToApi(dataSchema) {
    return {
      StudentAcademicEnrollmentID: dataSchema.studentAcademicEnrollmentID,
      StageID: dataSchema.stageID,
      StudentStageState: Number(dataSchema.studentStageState),
      StudentStageDate: dataSchema.studentStageDate,
    };
  }

  static mapUpdateToApi(dataSchema) {
    return {
      StudentStageID: dataSchema.studentStageID,
      StudentStageState: Number(dataSchema.studentStageState),
      StudentStageDate: dataSchema.studentStageDate,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      studentStageID: DataReq.StudentStageID,
      stageID: DataReq.StageID,
      stageName: DataReq.StageName,
      studentAcademicEnrollmentID: DataReq.StudentAcademicEnrollmentID,
      academicPlanName: DataReq.AcademicPlanName,
      studentName: DataReq.StudentName,
      studentStageState: DataReq.StudentStageState,
      studentStageStateValue: Number(DataReq.StudentStageStateValue),
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
        studentAcademicEnrollmentID: list.StudentAcademicEnrollmentID,
        academicPlanName: list.AcademicPlanName,
        studentName: list.StudentName,
        studentStageState: list.StudentStageState,
        studentStageStateValue: Number(list.StudentStageStateValue),
        studentStageDate: list.StudentStageDate,
      };
    });
  }
}
