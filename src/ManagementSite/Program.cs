using Microsoft.AspNetCore.HttpLogging;
using Repositories.Extensions;
using Serilog;
using Serilog.Events;

namespace ManagementSite;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        const string template = "{Timestamp:HH:mm:ss} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}";
        var logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: template)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker",
                LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .CreateLogger();
        builder.Services.AddControllers();
        builder.Services.AddSerilog(logger);
        builder.Services.AddHttpLogging(options =>
            options.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.Response);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Configuration.AddEnvironmentVariables();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("No connection string found");
        }

        builder.Services.AddDbContext(connectionString);
        builder.Services.AddRepositories();

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseHttpLogging();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}