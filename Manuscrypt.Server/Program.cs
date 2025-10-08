using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ManuscryptContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
});

builder.Services.AddScoped<ChannelRepo>();
builder.Services.AddScoped<CommentRepo>();
builder.Services.AddScoped<EditRepo>();
builder.Services.AddScoped<PostRepo>();
builder.Services.AddScoped<PostTagRepo>();
builder.Services.AddScoped<SubscriptionRepo>();
builder.Services.AddScoped<UserRepo>();

builder.Services.AddScoped<PostService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.UseSession();
app.Run();
