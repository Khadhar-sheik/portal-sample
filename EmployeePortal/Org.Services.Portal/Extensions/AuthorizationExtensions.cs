using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Org.Common.Model;

namespace EmployeePortal.Extensions
{
    public static class AuthorizationExtensions
    {
        private const string AUDIENCE = "EmployeePortal";
        public static void AddAuth(this IServiceCollection services)
        {
            var key = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes("asdqwe123"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.Audience = AUDIENCE;
                o.ClaimsIssuer = "EmployeePortal";

                o.TokenValidationParameters.ValidateIssuer = false;
                o.TokenValidationParameters.ValidateIssuerSigningKey = true;
                o.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
                o.TokenValidationParameters.RequireSignedTokens = true;
            });
            services.AddAuthorization(o =>
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder().RequireClaim(ClaimTypes.NameIdentifier).Build();
                
                o.AddPolicy("Administrator", b => b.RequireRole("Administrator"));
            });
        }

        public static string GenerateToken(this User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes("asdqwe123"));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Audience = AUDIENCE,
                Issuer = "EmployeePortal",
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}