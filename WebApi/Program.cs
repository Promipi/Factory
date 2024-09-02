
using Common.Core.Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using WebApi.Configuration.Settings;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"), config =>
                {
                    config.MigrationsAssembly("Infrastructure");
                });
            });

            builder.Services.AddCors(setupAction => setupAction.AddPolicy("FactoryPolicy", options =>
            {
                if (builder.Environment.IsDevelopment()) {
                    options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                } 
                else {
                    //If its not development, we need to specify the origins that are allowed to access the API from the appsetting.json
                }
                
            }));

            builder.Services.AddControllers();

            //Identity
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddIdentity<Customer, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = true;
                options.Lockout = new LockoutOptions() { AllowedForNewUsers = false };

            }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? string.Empty)),
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    // ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();
     
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion

            var app = builder.Build();

            #region Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
