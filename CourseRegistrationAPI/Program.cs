using Microsoft.EntityFrameworkCore;
using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<CourseRegistrationContext>(options =>
    options.UseInMemoryDatabase("CourseRegistrationDB"));

// Register Repository services
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Course Registration API",
        Version = "v1",
        Description = "A comprehensive API for managing course registration system including students, courses, instructors, departments, and registrations.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Course Registration System",
            Email = "support@courseregistration.edu"
        }
    });
    
    // Include XML comments for better documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure CORS for frontend integration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed the database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CourseRegistrationContext>();
    await DataSeeder.SeedDataAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course Registration API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        c.DocumentTitle = "Course Registration API Documentation";
        c.DefaultModelsExpandDepth(-1); // Collapse models by default
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Collapse operations by default
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

// Add a health check endpoint
app.MapGet("/health", () => new { 
    Status = "Healthy", 
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName,
    Version = "1.0.0"
})
.WithName("HealthCheck")
.WithOpenApi();

// Add API information endpoint
app.MapGet("/api/info", () => new
{
    Title = "Course Registration API",
    Version = "1.0.0",
    Description = "RESTful API for managing course registration system",
    Endpoints = new[]
    {
        "GET /api/students - Get all students",
        "GET /api/courses - Get all courses",
        "GET /api/instructors - Get all instructors",
        "GET /api/departments - Get all departments",
        "GET /api/registrations - Get all registrations",
        "GET /api/users - Get all users",
        "GET /swagger - API Documentation"
    },
    Database = "In-Memory",
    SampleData = "Auto-seeded on startup"
})
.WithName("ApiInfo")
.WithOpenApi();

Console.WriteLine("=== Course Registration API Started ===");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine("Swagger UI: https://localhost:7000 (when running in development)");
Console.WriteLine("Health Check: /health");
Console.WriteLine("API Info: /api/info");
Console.WriteLine("========================================");

app.Run();
