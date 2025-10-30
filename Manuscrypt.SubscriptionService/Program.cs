using Manuscrypt.Shared;
using Manuscrypt.SubscriptionService;
using Manuscrypt.SubscriptionService.Data;
using Manuscrypt.SubscriptionService.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SubscriptionContext>(options =>
    options.UseSqlite("Data Source=subscription.db"));

builder.Services.AddHttpClient<UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://userservice"); 
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<SubscriptionRepo>();
builder.Services.AddScoped<EventRepo>();
builder.Services.AddScoped<SubscriptionDomainService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SubscriptionContext>();
    db.Database.EnsureCreated();
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
