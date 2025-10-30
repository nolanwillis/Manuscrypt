using Manuscrypt.UserService;
using Manuscrypt.UserService.Data;
using Manuscrypt.UserService.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserContext>(options => options.UseSqlite("Data Source=user.db"));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<EventRepo>();
builder.Services.AddScoped<UserDomainService>();

var app = builder.Build();

// Ensure db is running and seed data.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserContext>();
    db.Database.EnsureCreated();
    await Seed.SeedUsersAsync(db, 10);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
