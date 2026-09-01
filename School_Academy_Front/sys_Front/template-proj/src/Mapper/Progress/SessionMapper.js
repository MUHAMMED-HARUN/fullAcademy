export default class SessionMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      SessionID: dataSchema.sessionID,
      SessionName: dataSchema.name,
      SessionType:
        dataSchema.type !== undefined && dataSchema.type !== null
          ? Number(dataSchema.type)
          : 0,
      SessionNote: dataSchema.note,
      ParentSessionID: dataSchema.parentSessionID,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      sessionID: DataReq.SessionID,
      name: DataReq.SessionName,
      type: DataReq.SessionType,
      typeName: DataReq.SessionTypeName,
      note: DataReq.SessionNote,
      parentSessionID: DataReq.ParentSessionID,
      parentSessionName: DataReq.ParentSessionName,
    };
  }

  static mapListApiToSessions(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        sessionID: list.SessionID,
        name: list.SessionName,
        type: list.SessionType,
        typeName: list.SessionTypeName,
        note: list.SessionNote,
        parentSessionID: list.ParentSessionID,
        parentSessionName: list.ParentSessionName,
      };
    });
  }
}
