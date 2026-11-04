namespace System.Application.Services.Institute.DTO
{
    public sealed class InstituteReadScope
    {
        public bool IsAdmin { get; init; }
        public Guid? TeacherID { get; init; }
        public Guid? StudentID { get; init; }
    }
}
