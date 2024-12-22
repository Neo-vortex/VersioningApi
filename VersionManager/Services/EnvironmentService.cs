using Dapper;
using VersionManager.Interfaces;
using Environment = VersionManager.Entities.Environment;

namespace VersionManager.Services;


public class EnvironmentService : IEnvironmentService
{
    private readonly DapperContext _context;

    public EnvironmentService(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Environment>> GetEnvironmentsByProjectAsync(int projectId)
    {
        const string query = "SELECT * FROM Environments WHERE ProjectId = @ProjectId;";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Environment>(query, new { ProjectId = projectId });
    }

    public async Task<int> CreateEnvironmentAsync(int projectId, string name)
    {
        const string query = @"
        INSERT INTO Environments (ProjectId, Name, CreatedAt)
        VALUES (@ProjectId, @Name, @CreatedAt);
        SELECT last_insert_rowid();";

        using var connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(query, new
        {
            ProjectId = projectId,
            Name = name,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateEnvironmentAsync(int projectId, int environmentId, string name)
    {
        const string query = @"
        UPDATE Environments
        SET Name = @Name, UpdatedAt = @UpdatedAt
        WHERE Id = @EnvironmentId AND ProjectId = @ProjectId;";

        using var connection = _context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query, new
        {
            ProjectId = projectId,
            EnvironmentId = environmentId,
            Name = name,
            UpdatedAt = DateTime.UtcNow
        });

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteEnvironmentAsync(int projectId, int environmentId)
    {
        const string query = @"
        DELETE FROM Environments
        WHERE Id = @EnvironmentId AND ProjectId = @ProjectId;";

        using var connection = _context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query, new
        {
            ProjectId = projectId,
            EnvironmentId = environmentId
        });

        return rowsAffected > 0;
    }
}