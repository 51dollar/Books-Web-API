using AspNetCoreRateLimit;
using CompanyEmployees.Extensions;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Utility;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using Service.DataShaping;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LogManager.Setup().LoadConfigurationFromFile("/nlog.config");

            builder.Services.ConfigureCors();
            builder.Services.ConfigureIISIntegration();
            builder.Services.ConfigureLoggerService();
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddScoped<ValidationFilterAttribute>();
            builder.Services.AddScoped<ValidateMediaTypeAttribute>();

            builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
            builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();

            builder.Services.ConfigureVersioning();
            builder.Services.ConfigureResponseCaching();
            builder.Services.ConfigureHttpCacheHeaders();

            builder.Services.AddMemoryCache();
            builder.Services.ConfigureRateLimitingOptions();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication();
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJWT(builder.Configuration);
            builder.Services.AddJwtConfiguration(builder.Configuration);

            builder.Services.ConfigureSwagger();

            builder.Services.AddControllers(config => {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
            }).AddXmlDataContractSerializerFormatters()
            .AddCustomCSVFormatter()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

            builder.Services.AddCustomMediaTypes();

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerManager>();
            app.ConfigureExceptionHandler(logger);

            if (app.Environment.IsProduction())
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseIpRateLimiting();
            app.UseCors("CorsPolicy");
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Company Employees API v1");
                s.SwaggerEndpoint("/swagger/v2/swagger.json", "Company Employees API v2");
            });

            app.MapControllers();

            app.Run();

            NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
                new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
                .Services.BuildServiceProvider()
                .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>().First();
        }
    }
}
