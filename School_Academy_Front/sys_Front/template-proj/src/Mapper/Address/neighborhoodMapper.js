export default class NeighborhoodMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      NeighborhoodID: dataSchema.id,
      NeighborhoodName: dataSchema.name,
      DistrictID: dataSchema.districtID,
    };
  }
  static MapFromApiDTO(DataReq) {
    return {
      id: DataReq.NeighborhoodID,
      name: DataReq.NeighborhoodName,
      districtID: DataReq.DistrictID,
    };
  }
  static mapListApiToNeighborhood(NeighborhoodList) {
    if (!NeighborhoodList) return [];
    return NeighborhoodList.map((neighborhood) => {
      return {
        id: neighborhood.NeighborhoodID,
        name: neighborhood.NeighborhoodName,
        districtID: neighborhood.DistrictID,
        districtName: neighborhood.DistrictName,
      };
    });
  }
}
