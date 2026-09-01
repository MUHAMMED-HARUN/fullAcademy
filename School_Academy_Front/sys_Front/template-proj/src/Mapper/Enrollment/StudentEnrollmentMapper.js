export default class StudentEnrollmentMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      EnrollmentID: dataSchema.enrollmentID,
      StudentID: dataSchema.studentID,
      GroupID: dataSchema.groupID,
      Date: dataSchema.date,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      enrollmentID: DataReq.EnrollmentID,
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
