import PersonMapper from "./PersonMapper";
export default class StudentMapper {
  static mapUpsertToApi(dataSchema) {
    const payload = {
      PersonID: dataSchema.personID,
    };
    if (dataSchema.studentID) payload.StudentID = dataSchema.studentID;
    return payload;
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      studentID: DataReq.StudentID,
      personID: DataReq.PersonID,
      ...PersonMapper.MapFromApiDTO(DataReq),
    };
  }
  static mapListApiToStudent(StudentList) {
    if (!StudentList) return [];

    // التحقق مما إذا كانت القائمة مصفوفة مباشرة أم كائن يحتوي على items القادم من Gridify
    const items = Array.isArray(StudentList)
      ? StudentList
      : StudentList.items || StudentList.data || [];

    return items.map((student) => StudentMapper.MapFromApiDTO(student));
  }
}
