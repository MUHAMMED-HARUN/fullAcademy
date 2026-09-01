using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Infrastructure.Identity;
using System.Security.Claims;
using System.Text;

namespace System.Infrastructure.Repositories.Auth
{
    public class JwtTokenGeneratorService : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGeneratorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(ApplicationUser user, IList<string> roles, IList<string> permissions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured.");
            var issuer = jwtSettings["Issuer"] ?? "SchoolAcademy";
            var audience = jwtSettings["Audience"] ?? "SchoolAcademyClient";

            var key = Encoding.ASCII.GetBytes(secretKey);

            // ✅ الأسماء المخصصة الجديدة
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),           // بدلاً من ClaimTypes.NameIdentifier
                new Claim("UserName", user.UserName),             // بدلاً من ClaimTypes.Name
                new Claim("Email", user.Email),                  // بدلاً من ClaimTypes.Email
                new Claim("EntityId", user.EntytyID.ToString()), // إذا أردت إضافة معرف الكيان
                new Claim("EntityType", user.EntytyType.ToString()) // نوع المستخدم (معلم/طالب/مدير)
            };

            // إضافة الأدوار (كـ Role واحد أو متعدد)
            foreach (var role in roles)
            {
                claims.Add(new Claim("Role", role)); // بدلاً من ClaimTypes.Role
            }

            // إضافة الصلاحيات (تبقى "Permission" كما هي)
            foreach (var per in permissions)
            {
                claims.Add(new Claim("Permission", per));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}