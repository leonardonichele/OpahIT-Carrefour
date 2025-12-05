using System.Reflection;
using MediatR;
using FluentValidation;
using GFICash.Domain.Messaging;
using GFICash.Infrastructure.Context;
using GFICash.Infrastructure.Messaging;
using GFICash.Infrastructure.Outbox;
using GFICash.Infrastructure.Repositories;
using GFICash.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddHostedService<OutboxDispatcher>();
builder.Services.AddTransient<KeyMiddleware>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gficash.db"));
builder.Services.AddScoped<ILaunchRepository, LaunchRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", p =>
    {
        p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddHttpClient();
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseDefaultTypeSerializer()
        .UseMemoryStorage());
builder.Services.AddHangfireServer();
builder.Services.AddSingleton<IMessageBroker, RabbitMqMessageBroker>();
builder.Services.AddHostedService<OutboxDispatcher>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        if (!context.Request.Query.TryGetValue("key", out var key) ||
            key != app.Configuration["ApiKey"])
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Key faltando na URL do Swagger");
            return;
        }
    }

    await next();
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapHealthChecks("/health");
app.UseMiddleware<KeyMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowMyOrigin");
app.UseHangfireDashboard("/jobs");
RecurringJob.AddOrUpdate<OutboxJob>(
    "outbox-dispatcher",
    job => job.ProcessAsync(),
    "* * * * *" // executa a cada 1 minuto
);
app.UseAuthorization();
app.MapControllers();
app.Run();