using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
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
            builder.Services.AddSingleton<CitiesDataStore>();

            builder.Services.AddDbContext<CityInfoContext>(dbContextOptions =>
                dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

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

            app.UseAuthorization();

            app.MapControllers();  // Where the selected endpoint is executed

            app.Run();
            #endregion Pipeline
        }
    }
}
