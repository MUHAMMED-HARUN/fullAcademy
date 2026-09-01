export default class TeachingAssignmentMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      EnrollmentID: dataSchema.enrollmentID,
      TeacherID: dataSchema.teacherID,
      GroupID: dataSchema.groupID,
      SubjectID: dataSchema.subjectID,
      Date: dataSchema.date,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      enrollmentID: DataReq.EnrollmentID,
      teacherID: DataReq.TeacherID,
      teacherName: DataReq.TeacherName,
      groupID: DataReq.GroupID,
      groupName: DataReq.GroupName,
      subjectName: DataReq.SubjectName,
      subjectID: DataReq.SubjectID,
      date: DataReq.EnrollmentDate,
      calssName: DataReq.CalssName,
      classLevel: DataReq.ClassLevel,
    };
  }

  static mapListApiToTeachers(dataList) {
    if (!dataList) return [];

    // التحقق مما إذا كانت القائمة مصفوفة مباشرة أم كائن يحتوي على items القادم من Gridify
    const items = Array.isArray(dataList)
      ? dataList
      : dataList.items || dataList.data || [];

    return items.map((list) => {
      return {
        enrollmentID: list.EnrollmentID,
        teacherID: list.TeacherID,
        teacherName: list.TeacherName,
        groupID: list.GroupID,
        groupName: list.GroupName,
        subjectName: list.SubjectName,
        subjectID: list.SubjectID,
        date: list.EnrollmentDate,
        calssName: list.CalssName,
        classLevel: list.ClassLevel,
      };
    });
  }
}
