using VersionManager.Entities;
using VersionManager.Interfaces;
using Dapper;
using Environment = VersionManager.Entities.Environment;

namespace VersionManager.Services;
public class ProjectService(DapperContext context) : IProjectService
{
    public async Task<int> CreateProjectAsync(string name)
    {
        const string query = "INSERT INTO Projects (Name) VALUES (@Name); SELECT last_insert_rowid();";
        using var connection = context.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(query, new { Name = name });
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        const string query = "SELECT * FROM Projects WHERE Id = @ProjectId";
        using var connection = context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Project>(query, new { ProjectId = projectId });
    }

    public async Task<int> CreateEnvironmentAsync(int projectId, string environmentName)
    {
        const string query = @"
        INSERT INTO Environments (ProjectId, Name, CreatedAt)
        VALUES (@ProjectId, @Name, @CreatedAt);
        SELECT last_insert_rowid();";

        using var connection = context.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(query, new
        {
            ProjectId = projectId,
            Name = environmentName,
            CreatedAt = DateTime.UtcNow
        });
    }

    // Get environments by project ID
    public async Task<IEnumerable<Environment>> GetEnvironmentsAsync(int projectId)
    {
        const string query = "SELECT * FROM Environments WHERE ProjectId = @ProjectId";
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Environment>(query, new { ProjectId = projectId });
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        const string query = "SELECT * FROM Projects";
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Project>(query);
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        const string deleteEnvironmentsQuery = "DELETE FROM Environments WHERE ProjectId = @ProjectId";
        using var connection = context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(deleteEnvironmentsQuery, new { ProjectId = projectId });

        const string deleteVersionTagsQuery = "DELETE FROM VersionTags WHERE ProjectId = @ProjectId";
        await connection.ExecuteAsync(deleteVersionTagsQuery, new { ProjectId = projectId });

        const string deleteProjectQuery = "DELETE FROM Projects WHERE Id = @ProjectId";
        rowsAffected += await connection.ExecuteAsync(deleteProjectQuery, new { ProjectId = projectId });

        return rowsAffected > 0; 
    }
}
