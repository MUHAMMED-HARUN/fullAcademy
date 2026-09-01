using System.Domain.Entities.Address;
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Person
{
    public class clsPerson : clsBaseClass
    {
        public string FirstName { get; private set; }
        public string FatherName { get; private set; }
        public string LastName { get; private set; }
        public string MotherName { get; private set; }
        public string MotherLastName { get; private set; }
        public string NationalNumber { get; private set; }
        public bool Gendor { get; private set; }
        public DateOnly BirthDate { get; private set; }
        
        // Nullable properties as per requirements (IDs usually represent FKs)
        // SystemUseCase.md mentions AddressID?, PlaceOfBirthID?, LivingPlaceID?
        // Assuming these are Int or Guid based on relation. If relations not defined yet, implementing as properties.
        // The reference used ints, but SystemUseCase implies Guid usage widely. 
        // However, usually Lookups might still be ints. I will use int? for lookups/FKs if not specified otherwise, 
        // but SystemUseCase.md didn't explicitly specify types for these IDs. 
        // Based on "guidID:baseClass" => BaseClass ID is Guid.
        // I will assume other FKs might be Guids or Ints. 
        // The reference "clsPeople" used `int? PlaceOfBirthID`. 
        // I'll stick to `Guid?` for consistency in a new Guid-based system, or `int?` if they are static lookups. 
        // Let's use `Guid?` for now to be safe with "Guid" requirement, or `int?` if they are small lookups.
        // Actually, SystemUseCase says "AddressID?", "PlaceOfBirthID?". 
        // I will use `Guid?` for consistency with "guidID" rule, unless they are simple Enums/Lookups.
        // Safe bet: Guid? given the instruction "guidID:baseClass".
        
        public Guid? PlaceOfBirthID { get; private set; }
        public Guid? LivingPlaceID { get; private set; }

        public clsCity? PlaceOfBirth { get; private set; }
        public clsAddress? LivingPlace { get; private set; }

        protected clsPerson() { } // For EF

        public clsPerson(string firstName, string fatherName, string lastName, 
                         string motherName, string motherLastName, string nationalNumber, 
                         bool gendor, DateOnly birthDate, 
                        Guid? placeOfBirthID = null, Guid? livingPlaceID = null)
        {
            // Set Base Properties (ID, CreatedBy, etc)
            // Ideally UserID comes from context, but for now assuming clean creation.
            // We need a way to pass "CreatedBy". I will add it to constructor or assume system context.
            // SystemUseCase says: "ينشئ ال baseclass و ينشئ له constroctor يقوم بتعيين القيم"
            // "Public Set methode لكل خاصية"
            
            // Validation
            if (string.IsNullOrWhiteSpace(firstName)) throw new DomainException(DomainExMsg.Person_FirstName);
            if (string.IsNullOrWhiteSpace(fatherName)) throw new DomainException(DomainExMsg.Person_FatherName);
            if (string.IsNullOrWhiteSpace(lastName)) throw new DomainException(DomainExMsg.Person_LastName);
            if (string.IsNullOrWhiteSpace(motherName)) throw new DomainException(DomainExMsg.Person_MotherName);
            if (string.IsNullOrWhiteSpace(motherLastName)) throw new DomainException(DomainExMsg.Person_MotherLastName);
            if (string.IsNullOrWhiteSpace(nationalNumber)) throw new DomainException(DomainExMsg.Person_NationalNumber);
            if (birthDate == default) throw new DomainException(DomainExMsg.Person_BirthDate);

            // Setting properties
            FirstName = firstName;
            FatherName = fatherName;
            LastName = lastName;
            MotherName = motherName;
            MotherLastName = motherLastName;
            NationalNumber = nationalNumber;
            Gendor = gendor;
            BirthDate = birthDate;
            PlaceOfBirthID = placeOfBirthID;
            LivingPlaceID = livingPlaceID;

            // Initialize Base (New ID, CreatedAt)
            // Note: In real app, we might pass the "User" who created this. 
            // For now, calling BaseSetCreated with empty or placeholder if not passed.
            // The instruction says "CreatedBy" is a column.
            // I'll assume we set it via the method later or pass it here.
            // For now, I will handle the "Structure" first.
            BaseSetCreated("System"); // Placeholder
        }

        public void SetFirstName(string value) 
        {
             if (string.IsNullOrWhiteSpace(value)) throw new DomainException(DomainExMsg.Person_FirstName);
             FirstName = value;
        }
        public void SetFatherName(string value) 
        {
             if (string.IsNullOrWhiteSpace(value)) throw new DomainException(DomainExMsg.Person_FatherName);
             FatherName = value;
        }
        public void SetLastName(string value) 
        {
             if (string.IsNullOrWhiteSpace(value)) throw new DomainException(DomainExMsg.Person_LastName);
             LastName = value;
        }
        public void SetMotherName(string value) 
        {
             if (string.IsNullOrWhiteSpace(value)) throw new DomainException(DomainExMsg.Person_MotherName);
             MotherName = value;
        }
        public void SetMotherLastName(string value) 
        {
             if (string.IsNullOrWhiteSpace(value)) throw new DomainException(DomainExMsg.Person_MotherLastName);
             MotherLastName = value;
        }
        public void SetNationalNumber(string value) 
        {
             if (string.IsNullOrWhiteSpace(value)) throw new DomainException(DomainExMsg.Person_NationalNumber);
             NationalNumber = value;
        }
        public void SetGender(bool value) 
        {
             Gendor = value;
        }
        public void SetBirthDate(DateOnly value) 
        {
             if (value == default) throw new DomainException(DomainExMsg.Person_BirthDate);
             BirthDate = value;
        }
        public void SetPlaceOfBirthID(Guid? value)
        {
            PlaceOfBirthID = value;
        }

        public void SetLivingPlaceID(Guid? value)
        {
            LivingPlaceID = value;
        }
        // ... and so on for others as per "كل الخواص التي في ال domains تكون privateSet" & "public Set methode" rules
    }
}
