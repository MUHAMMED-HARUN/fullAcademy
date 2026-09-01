
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Address;


public class clsAddress : clsBaseClass
{
    public Guid CountryID { get; private set; }
    public Guid CityID { get; private set; }
    public Guid DistrictID { get; private set; }
    public Guid NeighborhoodID { get; private set; }
    public string? Notes { get; private set; }

    public clsCountry Country { get; private set; }
    public clsCity City { get; private set; }
    public clsDistrict District { get; private set; }
    public clsNeighborhood Neighborhood { get; private set; }

    protected clsAddress() { }

    public clsAddress(Guid countryID, Guid cityID, Guid districtID, Guid neighborhoodID, string? notes)
    {
        if (countryID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_CountryID_Required);
        if (cityID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_CityID_Required);
        if (districtID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_DistrictID_Required);
        if (neighborhoodID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_NeighborhoodID_Required);

        CountryID = countryID;
        CityID = cityID;
        DistrictID = districtID;
        NeighborhoodID = neighborhoodID;
        Notes = notes;
        BaseSetCreated("System");
    }

    public void SetCountryID(Guid countryID)
    {
        if (countryID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_CountryID_Required);
        CountryID = countryID;
    }

    public void SetCityID(Guid cityID)
    {
        if (cityID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_CityID_Required);
        CityID = cityID;
    }

    public void SetDistrictID(Guid districtID)
    {
        if (districtID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_DistrictID_Required);
        DistrictID = districtID;
    }

    public void SetNeighborhoodID(Guid neighborhoodID)
    {
        if (neighborhoodID == Guid.Empty)
            throw new DomainException(DomainExMsg.Address_NeighborhoodID_Required);
        NeighborhoodID = neighborhoodID;
    }

    public void SetNotes(string? notes)
    {
        Notes = notes;
    }
}
