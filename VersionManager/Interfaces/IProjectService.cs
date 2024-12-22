using VersionManager.Entities;
using Environment = VersionManager.Entities.Environment;

namespace VersionManager.Interfaces;

public interface IProjectService
{
    Task<int> CreateProjectAsync(string name);
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task<int> CreateEnvironmentAsync(int projectId, string environmentName);
    Task<IEnumerable<Environment>> GetEnvironmentsAsync(int projectId);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<bool> DeleteProjectAsync(int projectId);
}
