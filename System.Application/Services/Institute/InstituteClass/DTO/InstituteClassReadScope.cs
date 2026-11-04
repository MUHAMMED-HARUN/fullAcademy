namespace System.Application.Services.Institute.InstituteClass.DTO
{
    public sealed class InstituteClassReadScope
    {
        public bool IsAdmin { get; init; }
        public Guid? TeacherID { get; init; }
        public Guid? StudentID { get; init; }
    }
}
