namespace System.Application.Services.Person.Teacher.DTO
{
    public class TeacherInfo
    {
        public string TeacherID { get; set; }
        public string PersonID{ get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string LastName { get; set; }
        public string MotherName { get; set; }
        public string MotherLastName { get; set; }
        public string NationalNumber { get; set; }
        public bool Gendor { get; set; }
        public DateOnly BirthDate { get; set; }
        public string PlaceOfBirth { get; set; } // Concat city and country
        public string LivingPlace { get; set; } // concat all address i string 
    }
}
