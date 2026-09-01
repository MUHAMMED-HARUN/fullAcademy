using System.Infrastructure.Identity;

namespace System.Infrastructure.Repositories.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user, IList<string> roles,IList<string>permissions) ;
    }
}