using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace D.SchoolManagementApi.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSecretKey = configuration["JwtSettings:SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
                options.AddPolicy("TeacherOnly", policy => policy.RequireRole("teacher"));
                options.AddPolicy("StudentOnly", policy => policy.RequireRole("student"));
            });
        }
    }
}
