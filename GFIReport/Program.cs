using GFIReport.Infrastructure.Persistence;
using GFIReport.Infrastructure.RabbitMq;
using GFIReport.Application.Interfaces;
using GFIReport.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReportDbContext>(opt =>
    opt.UseSqlite("Data Source=gfireport.db"));
builder.Services.AddTransient<KeyMiddleware>();
builder.Services.AddScoped<ILaunchReportRepository, LaunchReportRepository>();
builder.Services.AddScoped<ILaunchReportService, LaunchReportService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddSingleton<BatchProcessor>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<BatchProcessor>());
builder.Services.AddHostedService<LaunchCreatedConsumer>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SupportNonNullableReferenceTypes();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GFIReport", Version = "v1" });
});
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
    var db = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
    db.Database.EnsureCreated();
}

app.MapHealthChecks("/health");
app.UseMiddleware<KeyMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();