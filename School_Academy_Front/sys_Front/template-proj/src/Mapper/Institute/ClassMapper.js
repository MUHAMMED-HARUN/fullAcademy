export default class ClassMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      ClassID: dataSchema.classID,
      ClassName: dataSchema.name,
      ClassNotes: dataSchema.notes,
      ClassLevel: dataSchema.level,
      ProgramID: dataSchema.programID,
    };
  }

  static MapFromApiDTO(DataReq) {
    return {
      classID: DataReq.ClassID,
      name: DataReq.ClassName,
      notes: DataReq.ClassNotes,
      level: DataReq.ClassLevel,
      programID: DataReq.ProgramID,
      programName: DataReq.ProgramName,
    };
  }
  static mapListApiToClass(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        classID: list.ClassID,
        name: list.ClassName,
        notes: list.ClassNotes,
        level: list.ClassLevel,
        programID: list.ProgramID,
        programName: list.ProgramName,
      };
    });
  }
}
