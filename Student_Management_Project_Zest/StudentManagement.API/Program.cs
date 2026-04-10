using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Serilog;
using StudentManagement.API.Middleware;
using StudentManagement.Core.Interfaces;
using StudentManagement.Infrastructure.Data;
using StudentManagement.Infrastructure.Repositories;
using StudentManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "super_secret_key_that_is_long_enough_for_sha256!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    int retries = 6;
    while (retries > 0)
    {
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            if (!context.Users.Any())
            {
                context.Users.Add(new StudentManagement.Core.Entities.User
                {
                    Username = "admin",
                    PasswordHash = "admin123", // In a real app this must be hashed. Kept plaintext for simplicity per AuthController.
                    Role = "Admin"
                });
                context.SaveChanges();
            }
            Log.Information("Database migration and seeding completed successfully.");
            break; // Success, break out of loop
        }
        catch (Exception ex)
        {
            retries--;
            Log.Warning(ex, $"Database not ready yet. Retrying in 5 seconds... ({retries} attempts left)");
            if (retries == 0)
            {
                Log.Error(ex, "Critical failure: Could not migrate or seed database after maximum retries.");
            }
            System.Threading.Thread.Sleep(5000); // wait 5 seconds before retrying
        }
    }
}

app.Run();
