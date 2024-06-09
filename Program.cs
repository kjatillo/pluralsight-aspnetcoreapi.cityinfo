using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pluralsight.AspNetCoreWebApi.CityInfo.DbContexts;
using Pluralsight.AspNetCoreWebApi.CityInfo.Services;
using Serilog;

namespace Pluralsight.AspNetCoreWebApi.CityInfo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Serilog logger configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            #region Services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registers services for supporting controllers
            // ReturnHttpNotAcceptable - returns 406 response when the client requests an unsupported media type
            // AddNewtonsoftJson - input/output formatters for JSON and JSON PATCH that use JSON.NET
            // AddXmlDataContractSerializerFormatters - adds support for XML
            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
              .AddXmlDataContractSerializerFormatters();

            builder.Services.AddProblemDetails();  // Custom reponse can also be added

            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            builder.Services.AddDbContext<CityInfoContext>(dbContextOptions =>
                dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

            builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],  // Accepts token create by the API itself only
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
                    };
                });

#if DEBUG
            builder.Services.AddTransient<IMailService, LocalMailService>();
#else
            builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
            #endregion Services
            
            #region Pipeline
            var app = builder.Build();
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();  // Where the endpoint is selected

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();  // Where the selected endpoint is executed

            app.Run();
            #endregion Pipeline
        }
    }
}
