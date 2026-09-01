import person from "../Person/PersonMapper";
export default class PersonMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      StudentID: dataSchema.studentID,
      PersonID: dataSchema.personID,
    };
  }

  static MapFromApiDTO(DataReq) {
    return {
      studentID: DataReq.StudentID,
      personID: DataReq.PersonID,
      ...person.MapFromApiDTO(DataReq),
    };
  }
  static mapListApiToStudent(StudentList) {
    if (!StudentList) return [];

    // التحقق مما إذا كانت القائمة مصفوفة مباشرة أم كائن يحتوي على items القادم من Gridify
    const items = Array.isArray(StudentList)
      ? StudentList
      : StudentList.items || StudentList.data || [];

    return items.map((student) => {
      return {
        ...person.MapFromApiDTO(student),
        studentID: student.StudentID,
      };
    });
  }
}
