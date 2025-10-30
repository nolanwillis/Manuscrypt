using Microsoft.EntityFrameworkCore;
using Manuscrypt.PostService;
using Manuscrypt.PostService.Data;
using Manuscrypt.PostService.Data.Repositories;
using Manuscrypt.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostContext>(options => options.UseSqlite("Data Source=post.db"));

builder.Services.AddHttpClient<UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://userservice");
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<PostRepo>();
builder.Services.AddScoped<EventRepo>();
builder.Services.AddScoped<PostDomainService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostContext>();
    db.Database.EnsureCreated();
    await Seed.SeedPostsAsync(db, 10, 5);
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
