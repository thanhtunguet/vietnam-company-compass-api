using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

// Import our custom namespaces
using VietnamBusiness.Data;
using VietnamBusiness.Repositories;
using VietnamBusiness.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

// Set development mode for testing
builder.Environment.EnvironmentName = "Development";

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configure API behavior options
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
    options.SuppressMapClientErrors = false;
});

// Add DbContext
// Get connection string from environment variable first, then fallback to configuration
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? 
                      builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString) || builder.Environment.IsDevelopment())
{
    // Use in-memory database for development/testing if no connection string provided
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseInMemoryDatabase("VietnamBusinessDb");
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    });
    
    Console.WriteLine("Using in-memory database for development/testing");
}
else
{
    // Use SQL Server for production with provided connection string
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
        
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });
    
    Console.WriteLine("Using SQL Server with provided connection string");
}

// Register the repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register services
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<ICompanyStatusService, CompanyStatusService>();
builder.Services.AddScoped<ICrawlerJobService, CrawlerJobService>();
builder.Services.AddScoped<ICrawlingStatusService, CrawlingStatusService>();
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IApiUsageTrackingService, ApiUsageTrackingService>();
builder.Services.AddScoped<IWardService, WardService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyBusinessMappingService, CompanyBusinessMappingService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Vietnam Business API", 
        Version = "v1",
        Description = "A RESTful API for Vietnam business data",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@example.com"
        }
    });

    // Set the comments path for the Swagger JSON and UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// CORS policy
var enableCors = Environment.GetEnvironmentVariable("ENABLE_CORS")?.ToLower() == "true";
if (enableCors || builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });

        options.AddPolicy("ProductionPolicy", policy =>
        {
            policy.WithOrigins(
                  Environment.GetEnvironmentVariable("CORS_ORIGINS")?.Split(',') ?? 
                  new[] { "https://example.com" })
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
    
    Console.WriteLine("CORS is enabled");
}
else
{
    Console.WriteLine("CORS is disabled");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configure Swagger - available in all environments but at /swagger path
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vietnam Business API v1");
    c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger instead of root
});

// Apply CORS based on environment
if (enableCors || app.Environment.IsDevelopment())
{
    if (app.Environment.IsDevelopment())
    {
        app.UseCors("AllowAll");
    }
    else
    {
        app.UseCors("ProductionPolicy");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

app.Run();
