import person from "./PersonMapper";
export default class PersonMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      TeacherID: dataSchema.TeacherID,
      PersonID: dataSchema.personID,
    };
  }

  static MapFromApiDTO(DataReq) {
    return {
      TeacherID: DataReq.TeacherID,
      personID: DataReq.PersonID,
      ...person.MapFromApiDTO(DataReq),
    };
  }
  static mapListApiToTeacher(TeacherList) {
    if (!TeacherList) return [];
    return TeacherList.map((Teacher) => {
      return {
        ...person.MapFromApiDTO(Teacher),
        TeacherID: Teacher.TeacherID,
      };
    });
  }
}
