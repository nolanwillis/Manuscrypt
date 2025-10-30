using Microsoft.EntityFrameworkCore;
using Manuscrypt.CommentService;
using Manuscrypt.CommentService.Data;
using Manuscrypt.CommentService.Data.Repositories;
using Manuscrypt.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CommentContext>(options => options.UseSqlite("Data Source=comment.db"));

builder.Services.AddHttpClient<UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://userservice");
});

builder.Services.AddHttpClient<PostServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://postservice");
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<CommentRepo>();
builder.Services.AddScoped<EventRepo>();
builder.Services.AddScoped<CommentDomainService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CommentContext>();
    db.Database.EnsureCreated();
    await Seed.SeedCommentsAsync(db, 10, 10, 10);
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
