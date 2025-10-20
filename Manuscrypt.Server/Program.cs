using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ManuscryptContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<CommentRepo>();
builder.Services.AddScoped<PostRepo>();
builder.Services.AddScoped<SubscriptionRepo>();
builder.Services.AddScoped<UserRepo>();

builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

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
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ManuscryptContext>();
    await Seed.SeedUsersAndPostsAsync(db, 5, 15);
}

app.Run();
