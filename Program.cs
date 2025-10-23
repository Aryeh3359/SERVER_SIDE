using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.AddCors();

var app = builder.Build();

// Apply migrations and seed database at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // apply pending migrations (if any)
        context.Database.EnsureCreated();

        // Seed sample data if needed
        SeedData.SeedDatabase(context);
    }
    catch (Exception ex)
    {
        // Log the error in a real app. Keep startup going for dev simplicity.
        Console.WriteLine($"An error occurred seeding the DB: {ex.Message}");
    }
}

app.UseCors(
    policy => policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
);

app.MapControllers();

app.Run();
