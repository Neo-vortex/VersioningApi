using System.Data;
using Microsoft.Data.Sqlite;

namespace VersionManager;

public class DapperContext(IConfiguration configuration)
{
    private const string CreateProjectsTable = """
                                                   CREATE TABLE IF NOT EXISTS Projects (
                                                       Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                       Name TEXT NOT NULL
                                                   );
                                               """;

    private const string CreateVersionTagsTable = """
                                                      CREATE TABLE IF NOT EXISTS VersionTags (
                                                          Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                          ProjectId INTEGER NOT NULL,
                                                          EnvironmentId INTEGER NOT NULL,
                                                          Major INTEGER NOT NULL DEFAULT 0,
                                                          Minor INTEGER NOT NULL DEFAULT 0,
                                                          Patch INTEGER NOT NULL DEFAULT 0,
                                                          UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                          UNIQUE(ProjectId, EnvironmentId),
                                                          FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
                                                          FOREIGN KEY (EnvironmentId) REFERENCES Environments(Id) ON DELETE CASCADE
                                                      );
                                                  """;

    private const string CreateEnvTable = """
                                              CREATE TABLE IF NOT EXISTS Environments (
                                                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                  ProjectId INTEGER NOT NULL,
                                                  Name TEXT NOT NULL,
                                                  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                  UpdatedAt DATETIME,
                                                  UNIQUE(ProjectId, Name),
                                                  FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
                                              );
                                          """;

    private readonly string? _connectionString = configuration.GetConnectionString("SqlConnection");

    public IDbConnection CreateConnection()
        => new SqliteConnection(_connectionString);
    
    public void EnsureDatabaseCreated()
    {
        using var connection = CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        
        command.CommandText = CreateProjectsTable;
        command.ExecuteNonQuery();
        
        command.CommandText = CreateVersionTagsTable;
        command.ExecuteNonQuery();
        
        command.CommandText = CreateEnvTable;
        command.ExecuteNonQuery();
    }
}