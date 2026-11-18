export default class StudentEnrollmentMapper {
  static mapCreateToApi(dataSchema) {
    return {
      StudentAcademicEnrollmentID: dataSchema.studentAcademicEnrollmentID,
      GroupID: dataSchema.groupID,
      Date: dataSchema.date,
    };
  }

  static mapUpdateToApi(dataSchema) {
    return {
      EnrollmentID: dataSchema.groupEnrollmentID,
      Date: dataSchema.date,
    };
  }

  static mapRangeToApi(dataSchema) {
    return {
      StudentAcademicEnrollmentIDs: dataSchema.studentAcademicEnrollmentIDs,
      GroupID: dataSchema.groupID,
      Date: dataSchema.date,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      enrollmentID: DataReq.EnrollmentID,
      groupEnrollmentID: DataReq.EnrollmentID,
      studentAcademicEnrollmentID: DataReq.StudentAcademicEnrollmentID,
      academicPlanName: DataReq.AcademicPlanName,
      studentID: DataReq.StudentID,
      studentName: DataReq.StudentName,
      groupID: DataReq.GroupID,
      groupName: DataReq.GroupName,
      date: DataReq.Date,
    };
  }

  static mapListApiToClass(dataList) {
    if (!dataList) return [];

    // التحقق مما إذا كانت القائمة مصفوفة مباشرة أم كائن يحتوي على items القادم من Gridify
    const items = Array.isArray(dataList)
      ? dataList
      : dataList.items || dataList.data || [];

    return items.map((list) => {
      return {
        enrollmentID: list.EnrollmentID,
        groupEnrollmentID: list.EnrollmentID,
        studentAcademicEnrollmentID: list.StudentAcademicEnrollmentID,
        academicPlanName: list.AcademicPlanName,
        studentID: list.StudentID,
        studentName: list.StudentName,
        groupID: list.GroupID,
        groupName: list.GroupName,
        date: list.Date,
        canTrack: list.CanTrack,
        canNominate: list.CanNominate,
        canAddToStage: list.CanAddToStage,
      };
    });
  }
}
