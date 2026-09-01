
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsDistrict : clsBaseClass
    {
        public Guid CityID { get; private set; }
        public string DistrictName { get; private set; }

        public clsCity City { get; private set; }

        protected clsDistrict() { }

        public clsDistrict(Guid cityID, string districtName)
        {
            if (cityID == Guid.Empty)
                throw new DomainException(DomainExMsg.District_CityID_Required);
            if (string.IsNullOrWhiteSpace(districtName))
                throw new DomainException(DomainExMsg.District_Name_Required);

            CityID = cityID;
            DistrictName = districtName;
            BaseSetCreated("System");
        }

        public void SetCityID(Guid cityID)
        {
            if (cityID == Guid.Empty)
                throw new DomainException(DomainExMsg.District_CityID_Required);
            CityID = cityID;
        }

        public void SetDistrictName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(DomainExMsg.District_Name_Required);
            DistrictName = name;
        }
    }
}
