export default class GroupMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      GroupID: dataSchema.groupID,
      GroupName: dataSchema.groupName,
      Code: dataSchema.code,
      InstituteClassID: dataSchema.instituteClassID,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      groupID: DataReq.GroupID,
      groupName: DataReq.GroupName,
      code: DataReq.Code,
      instituteClassID: DataReq.InstituteClassID,
      className: DataReq.ClassName,
      instituteName: DataReq.InstituteName,
      hasActiveRelationships: !!DataReq.HasActiveRelationships,
    };
  }

  static mapListApiToClass(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        groupID: list.GroupID,
        groupName: list.GroupName,
        code: list.Code,
        instituteClassID: list.InstituteClassID,
        className: list.ClassName,
        instituteName: list.InstituteName,
        hasActiveRelationships: !!list.HasActiveRelationships,
      };
    });
  }
}
