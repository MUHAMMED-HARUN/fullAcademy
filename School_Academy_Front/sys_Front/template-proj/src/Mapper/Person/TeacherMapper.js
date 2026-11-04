import PersonMapper from "./PersonMapper";
export default class TeacherMapper {
  static mapUpsertToApi(dataSchema) {
    const payload = {
      PersonID: dataSchema.personID,
    };
    const teacherID = dataSchema.teacherID;
    if (teacherID) payload.TeacherID = teacherID;
    return payload;
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      teacherID: DataReq.TeacherID,
      personID: DataReq.PersonID,
      ...PersonMapper.MapFromApiDTO(DataReq),
    };
  }
  static mapListApiToTeacher(TeacherList) {
    if (!Array.isArray(TeacherList)) return [];
    return TeacherList.map((teacher) => TeacherMapper.MapFromApiDTO(teacher));
  }

  static mapPagingApiToTeacher(paging) {
    return {
      items: TeacherMapper.mapListApiToTeacher(paging?.Data),
      count: paging?.Count ?? 0,
    };
  }
}
