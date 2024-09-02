
using AutoMapper;
using Common.Core.Domain;
using Common.Core.Profiles;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Reflection;
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

            //Mapper
            builder.Services.AddSingleton(new MapperConfiguration(mapper =>
            {
                mapper.AddProfile<ProductProfile>();
                mapper.AddProfile<SaleProfile>();

            }).CreateMapper());

            builder.Services.AddAuthorization();
     
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            builder.Services.AddMemoryCache();

            #endregion

            var app = builder.Build();

            #region Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();



            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
