using System.Text;
using API.Data;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config){

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //         .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
            //                ValidateIssuerSigningKey = true,
            //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
            //                ValidateAudience = false 
            //         });
            services.AddIdentityCore<AppUser>(opt => { 
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.User.RequireUniqueEmail = false;
            }).AddRoles<AppRole>()
              .AddRoleManager<RoleManager<AppRole>>()
              .AddSignInManager<SignInManager<AppUser>>()
              .AddRoleValidator<RoleValidator<AppRole>>()
              .AddEntityFrameworkStores<DataContext>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => 
                    options.TokenValidationParameters = new TokenValidationParameters {
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                           ValidateAudience = false,
                           ValidateIssuer = false 
                    });
            services.AddAuthorization(opt => {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Roles.Admin.ToString()));
                opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole(Roles.Admin.ToString(), Roles.Moderator.ToString()));
            });
            return services;
        }
    }
}