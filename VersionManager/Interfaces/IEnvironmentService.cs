using Environment = VersionManager.Entities.Environment;

namespace VersionManager.Interfaces;

public interface IEnvironmentService
{
    Task<IEnumerable<Environment>> GetEnvironmentsByProjectAsync(int projectId);
    Task<int> CreateEnvironmentAsync(int projectId, string name);
    Task<bool> UpdateEnvironmentAsync(int projectId, int environmentId, string name);
    Task<bool> DeleteEnvironmentAsync(int projectId, int environmentId);
}