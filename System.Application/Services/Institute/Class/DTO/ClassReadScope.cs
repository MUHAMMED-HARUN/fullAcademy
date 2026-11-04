namespace System.Application.Services.Institute.Class.DTO
{
    public sealed class ClassReadScope
    {
        public bool IsAdmin { get; init; }
        public Guid? TeacherID { get; init; }
        public Guid? StudentID { get; init; }
    }
}
