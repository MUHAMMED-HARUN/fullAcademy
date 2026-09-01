//using School_Academy.Services;
//using System.Application;
//using System.Application.Abstractions.Auth;
//using System.Application.Services.Authentication.Permission.Query;
//using System.Infrastructure;
//var builder = WebApplication.CreateBuilder(args);


//    builder.Services.AddControllers(options =>
//{
//    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
//})
//.ConfigureApiBehaviorOptions(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//})
//.AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.PropertyNamingPolicy = null;
//});


//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
//builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.Services.AddApplication();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowVueApp", policy =>
//    {
//        policy.WithOrigins("http://localhost:8080", "http://192.168.1.13:8080") // —«»ÿ «·‹ Vue
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

//builder.Services.AddAuthorization(async options =>
//{
//    var handler = new GetAllPermissionHandler();
//    var permissionRes = await handler.Handle(null, CancellationToken.None);
//    foreach (var per in permissionRes.Value)
//    {
//        options.AddPolicy(per.Name, policy => policy.RequireClaim("Permission", per.Name));
//    }
//    options.AddPolicy("Permission", policy => policy.RequireAssertion(context =>
//    {
//        return context.User.HasClaim(c => c.Type == "Permission");
//    }));
//});

//var app = builder.Build();


//app.UseCors("AllowVueApp");

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SchoolSys API v1");
//        c.RoutePrefix = "swagger"; // «·«› —«÷Ì
//    });
//}
//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();

using School_Academy.Services;
using System.Application;
using System.Application.Abstractions.Auth;
using System.Application.Services.Authentication.Permission.Query;
using System.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. ≈÷«›… Œœ„«  «·„’«œﬁ… (Authentication) »«” Œœ«„ JWT Bearer
// ÌÃ» √‰ Ì „ –·ﬂ ﬁ»· «” œ⁄«¡ AddAuthorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // ﬁ—«¡… ≈⁄œ«œ«  JWT „‰ appsettings.json (ÌÃ» √‰  ﬂÊ‰ „ÊÃÊœ…)
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured.");
    var issuer = jwtSettings["Issuer"] ?? "SchoolAcademy";
    var audience = jwtSettings["Audience"] ?? "SchoolAcademyClient";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    // («Œ Ì«—Ì) · ”ÂÌ·  ’ÕÌÕ «·√Œÿ«¡ ›Ì »Ì∆… «· ÿÊÌ—
    if (builder.Environment.IsDevelopment())
    {
        options.RequireHttpsMetadata = false;
    }
});

// 2. ≈÷«›… Œœ„«  «· Õﬂ„ Ê«·‹ DI «·√Œ—Ï
builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplication();

// 3.  ”ÃÌ· Œœ„«  Swagger/OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // ≈÷«›… œ⁄„ JWT ›Ì Swagger (·  „ﬂ‰ „‰ «Œ »«— «·„’«œﬁ…)
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "√œŒ· «· Êﬂ‰ JWT ›Ì «·Õﬁ·: Bearer {token}"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 4. ”Ì«”«  CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:8080", "http://192.168.1.13:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ≈–« ﬂ‰   ” Œœ„ Cookies √Ê Ã·”« ° ·ﬂ‰ „⁄ JWT ·Ì” ÷—Ê—Ì« ⁄«œ…
    });
});

// 5. ”Ì«”«  «· ›ÊÌ÷ (Authorization)
builder.Services.AddAuthorization(async options =>
{
    //  Õ„Ì· «·’·«ÕÌ«  „‰ ﬁ«⁄œ… «·»Ì«‰«  √Ê „’œ— ¬Œ—
    var handler = new GetAllPermissionHandler();
    var permissionRes = await handler.Handle(null, CancellationToken.None);
    if (permissionRes?.Value != null)
    {
        foreach (var per in permissionRes.Value)
        {
            options.AddPolicy(per.Name, policy => policy.RequireClaim("Permission", per.Name));
        }
    }
    // ”Ì«”… ⁄«„…   ÿ·» ÊÃÊœ √Ì ’·«ÕÌ… (Permission)
    options.AddPolicy("Permission", policy => policy.RequireAssertion(context =>
    {
        return context.User.HasClaim(c => c.Type == "Permission");
    }));
});

var app = builder.Build();

// 6.  — Ì» Middlewares «·’ÕÌÕ
app.UseCors("AllowVueApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SchoolSys API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// 7.  — Ì» „Â„: Authentication ﬁ»· Authorization
app.UseAuthentication(); // Â–Â «·Œœ„…  ﬁÊ„ »›ﬂ «· Êﬂ‰ Ê„·¡ HttpContext.User
app.UseAuthorization();  // Â–Â «·Œœ„…  ﬁÊ„ » ÿ»Ìﬁ «·”Ì«”« 

app.MapControllers();

app.Run();
