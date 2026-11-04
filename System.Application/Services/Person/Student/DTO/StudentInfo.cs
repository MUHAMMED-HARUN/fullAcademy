namespace System.Application.Services.Person.Student.DTO
{
    public class StudentInfo
    {
        public string StudentID { get; set; } = string.Empty;
        public string PersonID { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string MotherLastName { get; set; } = string.Empty;
        public string NationalNumber { get; set; } = string.Empty;
        public bool Gendor { get; set; }
        public DateOnly BirthDate { get; set; }
        public string PlaceOfBirth { get; set; } = string.Empty; // Concat city and country
        public string LivingPlace { get; set; } = string.Empty; // concat all address i string
    }
}
