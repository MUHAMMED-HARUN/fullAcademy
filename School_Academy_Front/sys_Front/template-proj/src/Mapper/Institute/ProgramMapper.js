export default class ProgramMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      ProgramID: dataSchema.programID,
      ProgramName: dataSchema.name,
      ProgramNote: dataSchema.note,
    };
  }

  static MapFromApiDTO(DataReq) {
    return {
      programID: DataReq.ProgramID,
      name: DataReq.ProgramName,
      note: DataReq.ProgramNote,
    };
  }
  static mapListApiToProgram(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        programID: list.ProgramID,
        name: list.ProgramName,
        note: list.ProgramNote,
      };
    });
  }
}
