using Scalar.AspNetCore;
using VersionManager;
using VersionManager.Interfaces;
using VersionManager.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DapperContext>();
    context.EnsureDatabaseCreated();
}

app.MapOpenApi();
app.MapScalarApiReference("/docs", options =>
{
    options.WithTitle("API Version Manager");
});
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/docs") || context.Request.Path.StartsWithSegments("/openapi"))
    {
        await next(); 
        return;
    }

    if (!context.Request.Headers.TryGetValue("secret", out var value) ||
        value != (Environment.GetEnvironmentVariable("SECRET") ?? "changeme"))
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Forbidden: Invalid or missing 'secret' header.");
        return;
    }

    await next();
});
app.UseAuthorization();

app.MapControllers();

app.Run();