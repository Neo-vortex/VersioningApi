using Dapper;
using VersionManager.Entities;
using VersionManager.Interfaces;

namespace VersionManager.Services;

public class VersionService : IVersionService
{
    private readonly DapperContext _context;

    public VersionService(DapperContext context)
    {
        _context = context;
    }

    public async Task<VersionTag?> GetCurrentVersionAsync(int projectId, string environmentName)
    {
        const string query = @"
        SELECT vt.*
        FROM VersionTags vt
        INNER JOIN Environments e ON vt.EnvironmentId = e.Id
        WHERE vt.ProjectId = @ProjectId AND e.Name = @EnvironmentName;";
        
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<VersionTag>(query, new { ProjectId = projectId, EnvironmentName = environmentName });
    }

    public async Task<bool> SetVersionAsync(int projectId, string environmentName, int major, int minor, int patch)
    {
        const string query = @"
        INSERT INTO VersionTags (ProjectId, EnvironmentId, Major, Minor, Patch, UpdatedAt)
        VALUES (@ProjectId, 
                (SELECT Id FROM Environments WHERE Name = @EnvironmentName AND ProjectId = @ProjectId),
                @Major, @Minor, @Patch, @UpdatedAt)
        ON CONFLICT(ProjectId, EnvironmentId) DO UPDATE
        SET Major = @Major, Minor = @Minor, Patch = @Patch, UpdatedAt = @UpdatedAt;";

        using var connection = _context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query, new
        {
            ProjectId = projectId,
            EnvironmentName = environmentName,
            Major = major,
            Minor = minor,
            Patch = patch,
            UpdatedAt = DateTime.UtcNow
        });

        return rowsAffected > 0;
    }

    public async Task<VersionTag?> IncrementVersionAsync(int projectId, string environmentName, string part)
    {
        var version = await GetCurrentVersionAsync(projectId, environmentName);
        if (version == null) return null;

        switch (part.ToLower())
        {
            case "major":
                version.Major++;
                version.Minor = 0;
                version.Patch = 0;
                break;
            case "minor":
                version.Minor++;
                version.Patch = 0;
                break;
            case "patch":
                version.Patch++;
                break;
        }

        await SetVersionAsync(projectId, environmentName, version.Major, version.Minor, version.Patch);
        return version;
    }

    public async Task<VersionTag?> DecrementVersionAsync(int projectId, string environmentName, string part)
    {
        var version = await GetCurrentVersionAsync(projectId, environmentName);
        if (version == null) return null;

        switch (part.ToLower())
        {
            case "major":
                if (version.Major > 0) version.Major--;
                version.Minor = 0;
                version.Patch = 0;
                break;
            case "minor":
                if (version.Minor > 0) version.Minor--;
                version.Patch = 0;
                break;
            case "patch":
                if (version.Patch > 0) version.Patch--;
                break;
        }

        await SetVersionAsync(projectId, environmentName, version.Major, version.Minor, version.Patch);
        return version;
    }
}