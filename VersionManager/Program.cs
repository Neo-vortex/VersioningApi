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
app.MapScalarApiReference();

app.UseAuthorization();

app.MapControllers();

app.Run();