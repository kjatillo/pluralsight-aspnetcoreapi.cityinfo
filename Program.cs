using Microsoft.AspNetCore.StaticFiles;

namespace Pluralsight.AspNetCoreWebApi.CityInfo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registers services for supporting controllers
            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;  // return a 406 response when the client requests an unsupported media type
            }).AddNewtonsoftJson()  // Input/Output formatters for JSON and JSON PATCH that use JSON.NET
              .AddXmlDataContractSerializerFormatters();  // adds support for XML

            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            builder.Services.AddProblemDetails();  // Also add a better output when an exception is handled

            #region Manipulating Error Response
            //builder.Services.AddProblemDetails(options =>
            //{
            //    options.CustomizeProblemDetails = ctx =>
            //    {
            //        ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional Info Example");
            //        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
            //    };
            //});
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //}); // Old way of doing it

            app.Run();
        }
    }
}
