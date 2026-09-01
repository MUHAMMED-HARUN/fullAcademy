

using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsNeighborhood : clsBaseClass
    {
        public Guid DistrictID { get; private set; }
        public string NeighborhoodName { get; private set; }

        public clsDistrict District { get; private set; }

        protected clsNeighborhood() { }

        public clsNeighborhood(Guid districtID, string neighborhoodName)
        {
            if (districtID == Guid.Empty)
                throw new DomainException(DomainExMsg.Neighborhood_DistrictID_Required);
            if (string.IsNullOrWhiteSpace(neighborhoodName))
                throw new DomainException(DomainExMsg.Neighborhood_Name_Required);

            DistrictID = districtID;
            NeighborhoodName = neighborhoodName;
            BaseSetCreated("System");
        }

        public void SetDistrictID(Guid districtID)
        {
            if (districtID == Guid.Empty)
                throw new DomainException(DomainExMsg.Neighborhood_District_IDRequired);
            DistrictID = districtID;
        }

        public void SetNeighborhoodName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(DomainExMsg.Neighborhood_Name_Required);
            NeighborhoodName = name;
        }
    }
}
