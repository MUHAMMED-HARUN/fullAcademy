
using Microsoft.AspNetCore.Identity;
using System.Domain.Enums.Person;

namespace System.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid EntytyID { get; set; }
        public enUserType EntytyType { get; set; }

    }
}
