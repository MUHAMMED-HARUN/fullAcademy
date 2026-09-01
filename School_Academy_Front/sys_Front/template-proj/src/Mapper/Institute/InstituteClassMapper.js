export default class InstituteClassMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      InstituteClassID: dataSchema.instituteClassID,
      ClassID: dataSchema.classID,
      InstituteID: dataSchema.instituteID,
    };
  }

  static MapFromApiDTO(DataReq) {
    return {
      instituteClassID: DataReq.InstituteClassID,
      instituteID: DataReq.InstituteID,
      instituteName: DataReq.InstituteName,
      classID: DataReq.ClassID,
      className: DataReq.ClassName,
      classLevel: DataReq.ClassLevel,
    };
  }
  static mapListApiToInstituteClass(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        instituteClassID: list.InstituteClassID,
        instituteID: list.InstituteID,
        instituteName: list.InstituteName,
        classID: list.ClassID,
        className: list.ClassName,
        classLevel: list.ClassLevel,
      };
    });
  }
}
